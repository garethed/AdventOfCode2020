using System;
using System.Linq;

namespace AdventOfCode2020
{

    [System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = true)]
    public class TestAttribute : Attribute {

        object expected;
        object[] parameters;
        


        public TestAttribute(object expected, params object[] parameters) {
            this.expected = expected;
            this.parameters = parameters;
        }

        public static bool TestAnnotatedMethods(object target) {

            bool ret = true;

            foreach (var m in target.GetType().GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)) {
                foreach (var t in (TestAttribute[])m.GetCustomAttributes(typeof(TestAttribute), false)) {

                    try {

                        var p2 = m.GetParameters().Zip(t.parameters, (targetParam, value) => Convert.ChangeType(value, targetParam.ParameterType)).ToArray(); 
                        var o = m.Invoke(target, t.parameters);
                        var e2 = Convert.ChangeType(t.expected, m.ReturnType);
                        ret &= Utils.Assert(string.Join(',', p2.Select(x => x.ToString()).ToArray()), o.ToString(), e2.ToString());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        ret = false;
                        Utils.Assert(string.Join(',', t.parameters.Select(x => x.ToString()).ToArray()), "[test error]", t.expected.ToString());
                    }
                }
            }

            return ret;
        }
    }
}