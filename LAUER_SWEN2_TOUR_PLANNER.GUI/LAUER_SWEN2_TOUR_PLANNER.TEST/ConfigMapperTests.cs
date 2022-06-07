using LAUER_SWEN2_TOUR_PLANNER.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LAUER_SWEN2_TOUR_PLANNER.TEST
{
    public class ConfigMapperTests
    {
        [Test]
        public void Test_ConfigMapper()
        {
            ConfigMapper? mapper = null;

            using (var sr = new StreamReader("..\\..\\config.json"))
            {
                try
                {
                    mapper = JsonSerializer.Deserialize<ConfigMapper>(sr.ReadToEnd());
                    if (mapper == null)
                    {
                        Assert.Fail();
                    }
                }
                catch (Exception)
                {
                    Assert.Fail();
                }
            }

        }

        [Test]
        public void Test_ConfigMapper_DBString()
        {
            ConfigMapper? mapper = null;

            using (var sr = new StreamReader("..\\..\\config.json"))
            {
                try
                {
                    mapper = JsonSerializer.Deserialize<ConfigMapper>(sr.ReadToEnd());
                    if (mapper == null)
                    {
                        Assert.Fail();
                    }
                }
                catch (Exception)
                {
                    Assert.Fail();
                }
            }

            if(mapper.ConnectionString!=null && mapper.ConnectionString != "")
            {
                Assert.Pass();
            }
        }

        [Test]
        public void Test_ConfigMapper_DBUser()
        {
            ConfigMapper? mapper = null;

            using (var sr = new StreamReader("..\\..\\config.json"))
            {
                try
                {
                    mapper = JsonSerializer.Deserialize<ConfigMapper>(sr.ReadToEnd());
                    if (mapper == null)
                    {
                        Assert.Fail();
                    }
                }
                catch (Exception)
                {
                    Assert.Fail();
                }
            }


            if (mapper.DBUser != null && mapper.DBUser != "")
            {
                Assert.Pass();
            }
        }


        [Test]
        public void Test_ConfigMapper_DBPW()
        {
            ConfigMapper? mapper = null;

            using (var sr = new StreamReader("..\\..\\config.json"))
            {
                try
                {
                    mapper = JsonSerializer.Deserialize<ConfigMapper>(sr.ReadToEnd());
                    if (mapper == null)
                    {
                        Assert.Fail();
                    }
                }
                catch (Exception)
                {
                    Assert.Fail();
                }
            }

            if (mapper.DBPassword != null && mapper.DBPassword != "")
            {
                Assert.Pass();
            }
        }
    }
}
