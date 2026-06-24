namespace ABS
{
    // Observer para los cambios en la bitácora.
    // Lo implementan los formularios que muestran eventos y auditoría,
    // para refrescarse automáticamente cuando se registra cualquier evento
    // desde cualquier parte del sistema.
    public interface IObservadorBitacora
    {
        void ActualizarBitacora();
    }
}
