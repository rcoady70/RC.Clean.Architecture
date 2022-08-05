using System.Reflection;
using RC.CA.Domain.Entities.Club;
using Xunit.Sdk;

namespace RC.CA.WebApi.UnitTests.Caching;

public class CachingTestData : DataAttribute
{
    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        yield return new object[] { "7A15C368-3DC8-43D1-81FD-C6B18C37ABF3", MockMember, MockMember };
        yield return new object[] { "7A15C368-3DC8-43D1-81FD-777777777777", MockMember, MockMember };
    }
    /// <summary>
    /// Default mock data
    /// </summary>
    private Member MockMember { get; set; } = new Member()
    {
        Id = new Guid("7A15C368-3DC8-43D1-81FD-C6B18C37ABF3"),
        Name = "Tom Smith",
        Gender = "Male",
        Qualification = "Instructor",
        Experiences = new List<Experience>
                {
                    new Experience
                    {
                    //Id = 1,
                    MemberId = new Guid("7A15C368-3DC8-43D1-81FD-C6B18C37ABF3"),
                    QualificationName = "Level 2 instructor",
                    Description = "L2 kayak instructor open boat",
                    ExpiryDate = DateTime.Now.AddDays(365)
                },
                new Experience
                    {
                    //Id = 2,
                    MemberId = new Guid("7A15C368-3DC8-43D1-81FD-C6B18C37ABF3"),
                    QualificationName = "Level 3 instructor",
                    Description = "L3 kayak instructor open boat",
                    ExpiryDate = DateTime.Now.AddDays(365)
                }
                },
        PhotoUrl = ""
    };
}


