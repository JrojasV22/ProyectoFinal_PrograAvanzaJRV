namespace ProyectoFinal_RojasJeffry.Data
{
    // Calcula el nombre del cuatrimestre actual (I: ene-abr, II: may-ago, III: sep-dic).
    public static class Cuatrimestre
    {
        public static string Actual()
        {
            var hoy = DateTime.Now;
            var numero = hoy.Month <= 4 ? "I" : hoy.Month <= 8 ? "II" : "III";
            return $"{numero} Cuatrimestre {hoy.Year}";
        }
    }
}
