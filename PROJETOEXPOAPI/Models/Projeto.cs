namespace PROJETOEXPOAPI.Models
{
    public class Projeto
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Status { get; set; }
        public string? DatadeInicio { get; set; }
        public string? Tecnologias { get; set; }
        public string? Requisitos { get; set; }
        public string? Area { get; set; }
    }
}
