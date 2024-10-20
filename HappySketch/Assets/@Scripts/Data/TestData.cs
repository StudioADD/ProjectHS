using System.Collections.Generic;
using System;

namespace Data
{
    [Serializable]
    public class JTestData
    {
        public int DataId;
        public string TestName;
    }

    public class TestDataLoader : ILoader<int, JTestData>
    {
        public List<JTestData> Tests = new List<JTestData>();

        public Dictionary<int, JTestData> MakeDict()
        {
            Dictionary<int, JTestData> testDict = new Dictionary<int, JTestData>();
            foreach (JTestData test in Tests)
                testDict.Add(test.DataId, test);
            return testDict;
        }
    }
}