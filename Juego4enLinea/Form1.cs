using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Juego4enLinea
{
    public partial class Form1 : Form
    {
        //Se definen las constantes las cuales representan el numero de filas y columnas del juego

        private const int FILAS = 6;
        private const int COLUMNAS = 7;



        // Aqui se declaranlas variables e imagenes que utiliza el juego 

        private Image imagenJug1 = Image.FromFile(@"C:\Users\adili\Desktop\Juego4enLinea\Juego4enLinea\imagenes\mountains-1985027_640.jpg");
        private Image imagenJug2 = Image.FromFile(@"C:\Users\adili\Desktop\Juego4enLinea\Juego4enLinea\imagenes\water-4478793_640.jpg");



        // Se declara la matriz bidimensional que usa el tablero 
        // Pila usada para guardar el historial de movimientos de los jugadores
        // Cola utilizada para manejar los turnos de los jugadores

        private int[,] Tablero = new int[FILAS, COLUMNAS];               
        private Stack<int> HistorialMovimientos = new Stack<int>();     
        private Queue<int> ColaTurnos = new Queue<int>();               
       

        // se declara una variable booleana la cual deternmina si es el turno del jugador 1 o si es false jugador 2.

        private bool Turnojugador1 = true;                              
        private bool JuegoTermino = false;
       

        // Se declararon tres variables  las cuales se utilizan para llevar la cuenta de victorias de cada jugador o los empates

        private int contadorVictoriasJugador1 = 0;
        private int contadorVictoriasJugador2 = 0;
        private int contadorEmpates = 0;


        // Aqui se guarda los estados del juego en cada momento especifico.

        private Stack<int[,]> pilaEstadosJuego = new Stack<int[,]>();


        //Aqui se guarda los movimientos realizados por los jugadores en cada momeno del juego

        private Stack<Stack<int>> pilaHistorialMovimientos = new Stack<Stack<int>>();
        

        // Aqui se guarda los estados de turno del jugador 1

        private Stack<bool> pilaTurnosJugador1 = new Stack<bool>();
        

        // Aqui se almacena los estados del juego en diferentes momentos del juego, se determina si el juego ya a terminado o no

        private Stack<bool> pilaJuegoTerminado = new Stack<bool>();


        //Estas 3 pilas sirven para almacenar los contadores de Victorias de jugador 1 y 2 asi como los empates en diferentes momentos del juego.

        private Stack<int> pilaContadorVictoriasJugador1 = new Stack<int>();
        private Stack<int> pilaContadorVictoriasJugador2 = new Stack<int>();
        private Stack<int> pilaContadorEmpates = new Stack<int>();


        public Form1()
        {
            try
            {
                
                InitializeComponent();
                InicializarJuego();
                MostrarEstadisticas();
                btnReiniciar.Click += BtnReiniciar_Click;
                BtnGuardar.Click += BtnGuardar_Click;
                BtnCargar.Click += BtnCargar_Click;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al inicializar el juego: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InicializarJuego()
        {
            try
            {
                // Se cargan las imágenes de fichas de los jugadores

                imagenJug1 = Image.FromFile(@"C:\Users\adili\Desktop\Juego4enLinea\Juego4enLinea\imagenes\mountains-1985027_640.jpg");
                imagenJug2 = Image.FromFile(@"C:\Users\adili\Desktop\Juego4enLinea\Juego4enLinea\imagenes\water-4478793_640.jpg");

                // Se inicializa el tablero, el historial de movimientos y la cola de turnos

                Tablero = new int[FILAS, COLUMNAS];
                HistorialMovimientos = new Stack<int>();
                ColaTurnos = new Queue<int>();

                // Se inicializa cola de turnos de cada jugador

                ColaTurnos.Enqueue(1);
                ColaTurnos.Enqueue(2);

                // Crear botones en el panel para representar el tablero

                int margen = 10; // Margen entre los botones y los bordes del panel
                int botonWidth = 70;
                int botonHeight = 70;

                for (int i = 0; i < FILAS; i++)
                {
                    for (int j = 0; j < COLUMNAS; j++)
                    {
                        Button button = new Button
                        {
                            Width = botonWidth,
                            Height = botonHeight,
                            BackgroundImageLayout = ImageLayout.Stretch // Establecer la propiedad BackgroundImageLayout a StretchImage
                        };
                        button.Click += Button_Click;
                        button.Tag = j;
                        button.BackColor = Color.Aqua;
                        panelPrincipal.Controls.Add(button);

                        // Establecer la ubicación del botón con un margen
                        int xPos = margen + j * (botonWidth + margen);
                        int yPos = margen + i * (botonHeight + margen);
                        button.Location = new Point(xPos, yPos);
                    }
                }

                // Inicializar el tablero

                for (int i = 0; i < FILAS; i++)
                {
                    for (int j = 0; j < COLUMNAS; j++)
                    {
                        Tablero[i, j] = 0; // Esto representa una casilla vacía
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al inicializar el juego: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GuardarJuego()
        {
            try
            {
                // Guardar el estado actual del juego en las pilas
                pilaEstadosJuego.Push((int[,])Tablero.Clone());
                pilaHistorialMovimientos.Push(new Stack<int>(HistorialMovimientos));
                pilaTurnosJugador1.Push(Turnojugador1);
                pilaJuegoTerminado.Push(JuegoTermino);
                pilaContadorVictoriasJugador1.Push(contadorVictoriasJugador1);
                pilaContadorVictoriasJugador2.Push(contadorVictoriasJugador2);
                pilaContadorEmpates.Push(contadorEmpates);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el juego: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void CargarJuego()
        {
            try
            {
                // Verificar si hay estados de juego para cargar
                if (pilaEstadosJuego.Count > 0)
                {
                    // Restaurar el estado del juego desde las pilas
                    Tablero = pilaEstadosJuego.Pop();
                    HistorialMovimientos = pilaHistorialMovimientos.Pop();
                    Turnojugador1 = pilaTurnosJugador1.Pop();
                    JuegoTermino = pilaJuegoTerminado.Pop();
                    
                    contadorEmpates = pilaContadorEmpates.Pop();

                    // Mostrar el tablero y las estadísticas actualizadas
                    MostrarTablero();
                    MostrarEstadisticas();

                    // Actualizar visualmente el tablero
                    ActualizarTableroVisual();
                }
                else
                {
                    MessageBox.Show("No hay juegos guardados para cargar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el juego: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ReiniciarPartida()
        {
            try
            {
                // Reiniciar el tablero
                for (int i = 0; i < FILAS; i++)
                {
                    for (int j = 0; j < COLUMNAS; j++)
                    {
                        Tablero[i, j] = 0;
                    }
                }

                // Limpiar historial de movimientos
                HistorialMovimientos.Clear();

                // Limpiar la listBox de movimientos
                listBoxMovimientos.Items.Clear();

                // Mostrar el tablero actualizado
                MostrarTablero();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al reiniciar la partida: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void RealizarMovimiento(int columna)
        {
            // Verificar si la columna está llena
            if (ColumnaLlena(columna))
            {
                MessageBox.Show("¡La columna está llena! Elige otra columna.", "Columna llena", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Salir del método sin realizar ningún movimiento
            }

            try
            {
                // Encontrar la fila más baja disponible en la columna
                int fila = EncontrarFila(columna);

                // Realizar el movimiento si hay espacio disponible en la columna
                if (fila != -1)
                {
                    int jugador = Turnojugador1 ? 1 : 2;
                    Tablero[fila, columna] = jugador;
                    HistorialMovimientos.Push(columna * COLUMNAS + fila);
                    Turnojugador1 = !Turnojugador1;
                    ColaTurnos.Enqueue(ColaTurnos.Dequeue());
                    MostrarTablero();
                    ActualizarListBoxMovimientos();

                    // Verificar si hay un ganador o si se ha alcanzado un empate
                    if (ComprobarVictoria())
                    {
                        JuegoTermino = true;
                        MostrarEstadisticas();
                    }
                    else if (HistorialMovimientos.Count == FILAS * COLUMNAS)
                    {
                        JuegoTermino = true;
                        MostrarEstadisticas();
                        MessageBox.Show("Empate en la siguiente ronda.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al realizar el movimiento: {ex.Message}");
            }
        }

        private int EncontrarFila(int columna)
        {
            for (int fila = FILAS - 1; fila >= 0; fila--)
            {
                if (Tablero[fila, columna] == 0)
                {
                    return fila;
                }
            }
            return -1; // Si la columna está llena, devuelve -1
        }

        private bool ComprobarVictoria()
        {
            try
            {
                int jugador1 = 1;
                int jugador2 = 2;

                // Comprobar victoria en filas y columnas
                for (int i = 0; i < FILAS; i++)
                {
                    for (int j = 0; j < COLUMNAS; j++)
                    {
                        // Comprobar victoria horizontal
                        if (j <= COLUMNAS - 4 &&
                            (Tablero[i, j] == jugador1 || Tablero[i, j] == jugador2) &&
                            Tablero[i, j] == Tablero[i, j + 1] &&
                            Tablero[i, j] == Tablero[i, j + 2] &&
                            Tablero[i, j] == Tablero[i, j + 3])
                        {
                            ActualizarContadores(Tablero[i, j]);
                            MessageBox.Show($"¡Jugador {Tablero[i, j]} ha ganado! ¡Felicidades!");
                            return true;
                        }

                        // Comprobar victoria vertical
                        if (i <= FILAS - 4 &&
                            (Tablero[i, j] == jugador1 || Tablero[i, j] == jugador2) &&
                            Tablero[i, j] == Tablero[i + 1, j] &&
                            Tablero[i, j] == Tablero[i + 2, j] &&
                            Tablero[i, j] == Tablero[i + 3, j])
                        {
                            ActualizarContadores(Tablero[i, j]);
                            MessageBox.Show($"¡Jugador {Tablero[i, j]} ha ganado! ¡Felicidades!");
                            return true;
                        }
                    }
                }

                // Comprobar victoria en diagonales hacia la derecha
                for (int i = 0; i <= FILAS - 4; i++)
                {
                    for (int j = 0; j <= COLUMNAS - 4; j++)
                    {
                        if ((Tablero[i, j] == jugador1 || Tablero[i, j] == jugador2) &&
                            Tablero[i, j] == Tablero[i + 1, j + 1] &&
                            Tablero[i, j] == Tablero[i + 2, j + 2] &&
                            Tablero[i, j] == Tablero[i + 3, j + 3])
                        {
                            ActualizarContadores(Tablero[i, j]);
                            MessageBox.Show($"¡Jugador {Tablero[i, j]} ha ganado! ¡Felicidades!");
                            return true;
                        }
                    }
                }

                // Comprobar victoria en diagonales hacia la izquierda
                for (int i = 0; i <= FILAS - 4; i++)
                {
                    for (int j = 3; j < COLUMNAS; j++)
                    {
                        if ((Tablero[i, j] == jugador1 || Tablero[i, j] == jugador2) &&
                            Tablero[i, j] == Tablero[i + 1, j - 1] &&
                            Tablero[i, j] == Tablero[i + 2, j - 2] &&
                            Tablero[i, j] == Tablero[i + 3, j - 3])
                        {
                            ActualizarContadores(Tablero[i, j]);
                            MessageBox.Show($"¡Jugador {Tablero[i, j]} ha ganado! ¡Felicidades!");
                            return true;
                        }
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al comprobar la victoria: {ex.Message}");
                return false;
            }
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            GuardarJuego();
            MessageBox.Show("¡Has guardado la partida correctamente!", "Partida Guardada", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }



        // Manejador de eventos para el clic en el botón "btnCargar"
        private void BtnCargar_Click(object sender, EventArgs e)
        {
            CargarJuego();
            MessageBox.Show("¡Partida cargada correctamente!", "Partida Cargada", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }



        // Este metodo maneja el evento clic en el boton de reiniciar el cual llama al metodo reiniciar partida
        private void BtnReiniciar_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("¿Estás seguro de que deseas iniciar la partida?", "Confirmar Reinicio", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    ReiniciarPartida();
                    JuegoTermino = false; // Restablecer el estado del juego "en curso"
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al reiniciar la partida: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        // Aqui se maneja los eventos de clic en los botones del tablero, verifica que el juego no haya terminado y luego llama
        // al metodo realizar movimiento para poner la ficha en la columna que elija el usuario
        private void Button_Click(object sender, EventArgs e)
        {
            if (!JuegoTermino)
            {
                Button button = (Button)sender;
                int columna = (int)button.Tag;
                RealizarMovimiento(columna);

                // Después de que se realiza un movimiento, actualizar la etiqueta de turno
                MostrarEstadisticas();
            }
        }


        // Este metodo se encarga de actualizar los contadores de victorias de los jugadores asi como el de empates
        private void ActualizarContadores(int jugadorGanador)
        {
            if (jugadorGanador == 1)

                contadorVictoriasJugador1++;
            else if (jugadorGanador == 2)
                contadorVictoriasJugador2++;
            else
                contadorEmpates++;

        }

        private void ActualizarListBoxMovimientos()
        {
            try
            {
                listBoxMovimientos.Items.Clear();
                int movimiento = 1;

                foreach (var item in HistorialMovimientos)
                {
                    int columna = item / COLUMNAS;
                    int fila = item % COLUMNAS;
                    string jugador = movimiento % 2 == 1 ? "Jugador 1" : "Jugador 2";

                    listBoxMovimientos.Items.Add($"Movimiento {movimiento}: {jugador} - Columna {columna + 1}, Fila {FILAS - fila}");
                    movimiento++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar la lista de movimientos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ActualizarTableroVisual()
        {
            for (int i = 0; i < FILAS; i++)
            {
                for (int j = 0; j < COLUMNAS; j++)
                {
                    Button button = (Button)panelPrincipal.Controls[i * COLUMNAS + j];
                    if (Tablero[i, j] == 1)
                        button.BackgroundImage = imagenJug1;
                    else if (Tablero[i, j] == 2)
                        button.BackgroundImage = imagenJug2;
                    else
                        button.BackgroundImage = null;
                }
            }
        }


        private void MostrarTablero()
        {
            try
            {
                // Muestra las fichas en el tablero visual 
                for (int i = 0; i < FILAS; i++)
                {
                    for (int j = 0; j < COLUMNAS; j++)
                    {
                        Button button = (Button)panelPrincipal.Controls[i * COLUMNAS + j];
                        if (Tablero[i, j] == 1)
                            button.BackgroundImage = imagenJug1;
                        else if (Tablero[i, j] == 2)
                            button.BackgroundImage = imagenJug2;
                        else
                            button.BackgroundImage = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al mostrar el tablero: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarEstadisticas()
        {
            try
            {
                // Muestra las estadísticas de victorias y empates
                textBoxVictoriasJugador1.Text = $"Victorias Jugador 1: {contadorVictoriasJugador1}";
                textBoxVictoriasJugador2.Text = $"Victorias Jugador 2: {contadorVictoriasJugador2}";
                textBoxEmpates.Text = $"Empates: {contadorEmpates}\r\n";

                // Muestra de quién es el turno actual
                if (!JuegoTermino)
                {
                    if (Turnojugador1)
                    {
                        labelTurno.Text = "Turno: Jugador 1";
                    }
                    else
                    {
                        labelTurno.Text = "Turno: Jugador 2";
                    }
                }
                else
                {
                    labelTurno.Text = "Juego terminado";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al mostrar las estadísticas: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarTableroVisual()
        {
            foreach (Control control in panelPrincipal.Controls)
            {
                Button button = control as Button;
                if (button != null)
                {
                    button.BackgroundImage = null;
                }
            }
        }
        private bool ColumnaLlena(int columna)
        {
            for (int i = 0; i < FILAS; i++)
            {
                if (Tablero[i, columna] == 0)
                {
                    // Todavía hay espacio disponible en esta columna
                    return false;
                }
            }
            // La columna está completamente llena
            return true;
        }

        private void BtnCargar_Click_1(object sender, EventArgs e)
        {

        }

        private void btnReiniciar_Click_1(object sender, EventArgs e)
        {

        }

        private void textBoxVictoriasJugador1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}