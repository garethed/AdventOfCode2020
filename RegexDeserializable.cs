﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Reflection;

namespace AdventOfCode2020
{

    [System.AttributeUsage(System.AttributeTargets.Class |
                           System.AttributeTargets.Struct | AttributeTargets.Field)
    ]
    public class RegexDeserializable : System.Attribute
    {
        private string regex;

            public RegexDeserializable(string regex)
        {
            this.regex = regex;
        }

        public static List<T> Deserialize<T>(string serialized)
        {
            RegexDeserializable[] attributes = (RegexDeserializable[]) typeof(T).GetCustomAttributes(typeof(RegexDeserializable), false);
            var regex = attributes[0].regex;

            Regex r = new Regex(regex, RegexOptions.Singleline | RegexOptions.Multiline);
            List<T> results = new List<T>();

            var matches = r.Matches(serialized);

            foreach (Match match in matches)
            {
                T output;

                foreach (var constructor in typeof(T).GetConstructors()) 
                {
                    var parameters = new List<object>();
                    if (r.GetGroupNames().Length - 1 == constructor.GetParameters().Length)
                    {
                        foreach (var parameter in constructor.GetParameters()) 
                        {
                            foreach (var groupName in r.GetGroupNames().Skip(1))
                            {
                                if (groupName == parameter.Name)
                                {
                                    string value = match.Groups[groupName].Value;
                                    var converted = Convert.ChangeType(value, parameter.ParameterType);
                                    parameters.Add(converted);
                                }
                            }

                        }
                    }

                    if (parameters.Count == constructor.GetParameters().Length)
                    {
                        output = (T)constructor.Invoke(parameters.ToArray());
                        goto NextObject;
                    }

                }



                output = Activator.CreateInstance<T>();

                foreach (var groupName in r.GetGroupNames().Skip(1))
                {
                    FieldInfo fi = typeof(T).GetField(groupName);
                    string value = match.Groups[groupName].Value;
                    var converted = Convert.ChangeType(value, fi.FieldType);
                    fi.SetValue(output, converted);

                }

                foreach (var field in typeof(T).GetFields())
                {
                    RegexDeserializable[] fieldAttributes = (RegexDeserializable[])field.GetCustomAttributes(typeof(RegexDeserializable), false);
                    if (fieldAttributes.Any())
                    {
                        var fieldRegex = new Regex(fieldAttributes[0].regex, RegexOptions.Singleline | RegexOptions.Multiline);

                        foreach (Match fieldMatch in fieldRegex.Matches(match.Groups[0].Value))
                        {
                            var converted = Convert.ChangeType(fieldMatch.Groups[1].Value, field.FieldType);
                            field.SetValue(output, converted);
                        }
                    }
                }

            NextObject:
                results.Add(output);

            }

            return results;
        }
    }
}
