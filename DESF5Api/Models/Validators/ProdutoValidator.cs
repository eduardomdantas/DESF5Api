using FluentValidation;

namespace DESF5Api.Models.Validators
{
    public class ProdutoValidator : AbstractValidator<Produto>
    {
        public ProdutoValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("O nome é obrigatório")
                .Length(3, 100).WithMessage("O nome deve ter entre 3 e 100 caracteres");

            RuleFor(x => x.Descricao)
                .MaximumLength(500).WithMessage("A descrição deve ter no máximo 500 caracteres");

            RuleFor(x => x.Preco)
                .GreaterThan(0).WithMessage("O preço deve ser maior que zero");
        }
    }
}