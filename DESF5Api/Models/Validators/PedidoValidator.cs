using FluentValidation;

namespace DESF5Api.Models.Validators
{
    public class PedidoValidator : AbstractValidator<Pedido>
    {
        public PedidoValidator()
        {
            RuleFor(x => x.ClienteId)
                .GreaterThan(0).WithMessage("ClienteId inválido");

            RuleFor(x => x.Itens)
                .NotEmpty().WithMessage("O pedido deve ter pelo menos um item")
                .Must(items => items.All(i => i.Quantidade > 0))
                .WithMessage("A quantidade de cada item deve ser maior que zero");
        }
    }
}