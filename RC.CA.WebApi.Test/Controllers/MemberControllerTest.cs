namespace RC.CA.WebApi.Test.Controllers
{

    public class MemberControllerTest
    {
        public MemberControllerTest()
        {

        }
        [Test]
        public async Task MemberController_Get_OkResponse()
        {
            var moqMemberRepository = Moq<IMemberRepository>();
            var moqMapper = Moq<IMemberRepository>();
            var getMemberRequest = new GetMemberRequestHandler(moqMemberRepository, moqMapper);
            getMemberRequest.Get

            //https://www.youtube.com/watch?v=C4hvWJqju7s
        }
    }
}
