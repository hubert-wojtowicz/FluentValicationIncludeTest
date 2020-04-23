using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FluentValidationIncludeTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly IValidator<Child> _validator;

        public TestController(
            ILogger<TestController> logger,
            IValidator<Child> validator)
        {
            _logger = logger;
            _validator = validator;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Child request)
        {
            var validationRes = await _validator.ValidateAsync(request);
            if (!(validationRes).IsValid)
            {
                return BadRequest(validationRes.Errors);
            }
            return Ok("Modle passes validation");
        }
    }

    public class Parent
    {
        public int Number { get; set; }
    }

    public class Child : Parent
    {
        public string Name { get; set; }
    }

    public class ParentValidator : AbstractValidator<Parent>
    {
        public ParentValidator()
        {
            RuleFor(x => x.Number)
                .GreaterThan(18);
        }
    }

    public class ChildValidator : AbstractValidator<Child>
    {
        public ChildValidator(IValidator<Parent> baseClassValidator)
        {
            Include(baseClassValidator);

            RuleFor(x => x.Name)
               .MinimumLength(5);
        }
    }
}
