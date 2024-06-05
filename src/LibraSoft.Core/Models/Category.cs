using LibraSoft.Core.Commons;
using LibraSoft.Core.Models.Validations;

namespace LibraSoft.Core.Models
{
    public class Category : ModelBase
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public IEnumerable<Book> Books { get; set; } = new List<Book>();

        protected Category() { }

        public Category(string title)
        {
            Title = title;

            this.Validate();
        }

        protected override void Validate()
        {
            var validator = new CategoryValidate();

            var validate = validator.Validate(this);

            ThrowErrorInValidate(validate);
        }
    }
}
