namespace LoginBlazor.Models
{
    public class User
    {
        public string Usuario { get; set; }
        public string Dominio { get; set; }
        public string Cidade { get; set; }
        public int AnoNascimento { get; set; }
        public string Funcao { get; set; }
        public int NivelSeguranca { get; set; }
        public string Senha { get; set; }
    }
}