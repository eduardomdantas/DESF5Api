using FluentValidation;

namespace DESF5Api.Models.Validators
{
    public class ClienteValidator : AbstractValidator<Cliente>
    {
        public ClienteValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("O nome é obrigatório")
                .Length(3, 100).WithMessage("O nome deve ter entre 3 e 100 caracteres");

            RuleFor(x => x.CPF)
                .NotEmpty().WithMessage("O CPF é obrigatório")
                .Length(11).WithMessage("CPF deve ter 11 caracteres")
                .Must(ValidarCPF).WithMessage("CPF inválido");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O e-mail é obrigatório")
                .EmailAddress().WithMessage("E-mail inválido");
        }

        private bool ValidarCPF(string cpf)
        {
            // Implementação simplificada da validação de CPF
            if (string.IsNullOrWhiteSpace(cpf)) return false;

            cpf = cpf.Trim().Replace(".", "").Replace("-", "");

            if (cpf.Length != 11) return false;

            // Verifica se todos os dígitos são iguais
            if (cpf.All(c => c == cpf[0])) return false;

            // Aqui você pode implementar a validação real dos dígitos verificadores
            return true;
        }
    }
}