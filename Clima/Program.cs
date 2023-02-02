using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clima
{
    class Program
    {
        static void Main(string[] args)
        {
            EstacionMetereologica estacionMetereologica = new EstacionMetereologica();

            DispositivoTiempoActual dispositivoTiempoActual = new DispositivoTiempoActual();
            DispositivoEstadisticas dispositivoEstadisticas = new DispositivoEstadisticas();
            DispositivoPredictivo dispositivoPredictivo = new DispositivoPredictivo();

            estacionMetereologica.HaCambiadoElTiempo += dispositivoTiempoActual.ActualizarPantallaDipositivo;
            estacionMetereologica.HaCambiadoElTiempo += dispositivoEstadisticas.AñadirDatosParaLasEstadisticas;
            estacionMetereologica.HaCambiadoElTiempo += dispositivoPredictivo.AñadirDatosDePrediccion;

            estacionMetereologica.AumentarLaTemperaturaEnGrados(1);

            Console.ReadLine();
        }
    }
    public interface IPublicador
    {
        void RegistrarObservador(IObservador observador);
        void QuitarObservador(IObservador observador);

        void Notificar();
    }
    public interface IObservador
    {
        void Actualizar();
    }
    /*
    public class EstacionMetereologica
    {
        public decimal Temperatura { get; }
        public decimal Presion { get; }
        public decimal Humedad { get; }

        public void HaCambiadoAlgunaMedida()
        {

        }

    }*/
    /*public class EstacionMetereologicaVersion1
    {
        public decimal Temperatura { get; }
        public decimal Presion { get; }
        public decimal Humedad { get; }


        private readonly DispositivoTiempoActual _dispositivoTiempoActual;
        private readonly DispositivoEstadisticas _dispositivoEstadisticas;
        private readonly DispositivoPredictivo _dispositivoPredictivo;

        public EstacionMetereologicaVersion1(
                DispositivoTiempoActual dispositivoTiempoActual,
                DispositivoEstadisticas dispositivoEstadisticas,
                DispositivoPredictivo dispositivoPredictivo
            )
        {
            _dispositivoTiempoActual = dispositivoTiempoActual;
            _dispositivoEstadisticas = dispositivoEstadisticas;
            _dispositivoPredictivo = dispositivoPredictivo;
        }

        public void HaCambiadoElTiempo()
        {
            _dispositivoTiempoActual.Actualizar(Temperatura, Presion, Humedad);
            _dispositivoEstadisticas.Actualizar(Temperatura, Presion, Humedad);
            _dispositivoPredictivo.Actualizar(Temperatura, Presion, Humedad);
        }
    }*/
    public class EstacionMetereologica : IPublicador
    {
        public decimal Temperatura { get; }
        public decimal Presion { get; }
        public decimal Humedad { get; }

        private readonly List<IObservador> _observadores;

        public EstacionMetereologica(List<IObservador> observadores)
        {
            _observadores = observadores;
        }

        public void Notificar()
        {
            foreach (var observador in _observadores)
            {
                observador.Actualizar(Temperatura, Presion, Humedad);
            }
        }
        public void HaCambiadoElTiempo()
        {
            Notificar();
        }

        public void RegistrarObservador(IObservador observador)
        {
            _observadores.Add(observador);
        }

        public void QuitarObservador(IObservador observador)
        {
            _observadores.Remove(observador);
        }
    }
    public class DispositivoTiempoActual : IObservador
    {
        private readonly EstacionMetereologica _estacionMetereologica;

        public DispositivoTiempoActual(EstacionMetereologica estacionMetereologica)
        {
            _estacionMetereologica = estacionMetereologica;
        }

        public void Actualizar()
        {
            var temperatura = _estacionMetereologica.Temperatura;
            var presion = _estacionMetereologica.Presion;
            var humedad = _estacionMetereologica.Humedad;
        }
    }
    public class EstacionMetereologica
    {
        public decimal Temperatura { get; private set; }
        public decimal Presion { get; private set; }
        public decimal Humedad { get; private set; }

        public event EventHandler<Tuple<decimal, decimal, decimal>> HaCambiadoElTiempo;

        public void AumentarLaTemperaturaEnGrados(int grados)
        {
            Temperatura = grados + 1;

            Notificar();
        }

        public void Notificar()
        {
            var medidas = new Tuple<decimal, decimal, decimal>(Temperatura, Humedad, Presion);

            if (HaCambiadoElTiempo != null)
                HaCambiadoElTiempo.Invoke(this, medidas);
        }
    }
}
