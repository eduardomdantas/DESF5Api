using FluentValidation;

namespace DESF5Api.Models.Validators
{
    public class PedidoItemValidator : AbstractValidator<PedidoItem>
    {
        public PedidoItemValidator()
        {
            RuleFor(x => x.PedidoId)
                .GreaterThan(0).WithMessage("Identificador do pedido inválido");

            RuleFor(x => x.ProdutoId)
                .GreaterThan(0).WithMessage("Identificador do produto inválido");

            RuleFor(x => x.Quantidade)
                .GreaterThan(0).WithMessage("Quantidade deve ser maior que zero")
                .LessThanOrEqualTo(1000).WithMessage("Quantidade máxima excedida");

            RuleFor(x => x.PrecoUnitario)
                .GreaterThan(0).WithMessage("Preço unitário inválido");
        }
    }
}
