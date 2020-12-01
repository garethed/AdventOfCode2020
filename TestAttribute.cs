using System;
using System.Linq;

namespace AdventOfCode2020
{

    [System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = true)]
    public class TestAttribute : Attribute {

        object e;
        object[] p;
        


        public TestAttribute(object e, params object[] p) {
            this.e = e;
            this.p = p;
        }

        public static bool TestAnnotatedMethods(object target) {

            bool ret = true;

            foreach (var m in target.GetType().GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)) {
                foreach (var t in (TestAttribute[])m.GetCustomAttributes(typeof(TestAttribute), false)) {

                    try {

                        var p2 = m.GetParameters().Zip(t.p, (targetParam, value) => Convert.ChangeType(value, targetParam.ParameterType)).ToArray(); 
                        var o = m.Invoke(target, t.p);
                        var e2 = Convert.ChangeType(t.e, m.ReturnType);
                        ret &= Utils.Assert(string.Join(',', p2.Select(x => x.ToString()).ToArray()), o.ToString(), e2.ToString());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        ret = false;
                        Utils.Assert(string.Join(',', t.p.Select(x => x.ToString()).ToArray()), "[test error]", t.e.ToString());
                    }
                }
            }

            return ret;
        }
    }
}