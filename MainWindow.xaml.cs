using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Beginners09_WPF_TicTacToe {
    

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        #region Private Members

        const int GRIDROWS = 3;
        const int GRIDCOLS = 3;

        private byte[,] cellValues;
        private bool player1Turn;
        private int winner;
        private bool gameEnded;
        private int numCellsFilled;

        #endregion

        #region Constructors
        public MainWindow() {
            InitializeComponent();

            ResetGame();
        }

        #endregion




        private void ResetGame() {

            // initialize the cellValues array so that all cells are free
            cellValues = new byte[GRIDROWS, GRIDCOLS];
            for(int i = cellValues.GetLength(0)-1; i > -1; --i) {
                for(int k = cellValues.GetLength(1)-1; k > -1; --k) {
                    cellValues[i, k] = (byte)MarkType.Free;
                }
            }

            // no cells have been filled with X or O
            numCellsFilled = 0;

            // ensure that the game starts with player1's turn
            player1Turn = true;

            // the game is new, therefore the game is not ended
            gameEnded = false;

            // iterate every button on the grid....
            grid_BtnContainer.Children.Cast<Button>().ToList().ForEach(button => {
                button.Content = string.Empty;
                button.Background = Brushes.White;
                button.Foreground = Brushes.Blue;
            });
            

        }

        // Message handler for main game grid box clicks
        private void Button_Click(object sender, RoutedEventArgs e) {

            // if the game is over, clicking on a main game grid button will reset the game
            if (gameEnded) {
                ResetGame();
                return;
            }

            // get sender data
            var button = (Button)sender;        // cast sender to the Button class
            var col = Grid.GetColumn(button);   // get the grid-column attached property value
            var row = Grid.GetRow(button);      // get the grid-row attached property value

            // get the appropriate cell value
            var flag = cellValues[row, col];

            // exit click handler if the clicked button is not free
            if ((MarkType)flag != MarkType.Free) {
                return;
            }

            numCellsFilled++;
            
            // process player1's turn
            if (player1Turn) {

                // process player1 clicking on a free space

                button.Foreground = Brushes.Blue;
                button.Content = "X";                       // update UI text with X
                cellValues[row, col] = (byte)MarkType.X;    // update underlying 2D byte array with enum flag
                player1Turn = false;                        // switch to player2's turn

            }
            else {

                // process player2 clicking on a free space

                button.Foreground = Brushes.Red;
                button.Content = "O";                       // update UI text with O
                cellValues[row, col] = (byte)MarkType.O;    // update underlying 2D byte array with enum flag
                player1Turn = true;                         // switch to player1's turn
            }


            if (CheckForAWinner()) {
                gameEnded = true;
                numCellsFilled = 0;

                int loser = winner == 1 ? 2 : 1;

                MessageBox.Show($"Congratulations!! Player{winner}\nBetter Luck next time Player{loser} :( ..");
            }
            else if(numCellsFilled == (GRIDCOLS * GRIDROWS)){

                // do game ended without winner condition here

                gameEnded = true;
                MessageBox.Show("You both suck..............\nI am disappointed.");

            }

        }

        /// <summary>
        /// Check if a win condition has occurred and assign the winner if so.
        /// Use code that could be expanded to any size of grid cuz why the hell not !!
        /// </summary>
        /// <returns></returns>
        private bool CheckForAWinner() {

            #region variables
            int r; // reusable loop variable for rows
            int c; // reusable loop variable for cols
            MarkType m = MarkType.Free; // variable to check cell values
            #endregion

            #region investigate each row
            // iterate through rows
            for (r = 0; r < GRIDROWS; r++) {
                

                // iterate through cols of current row
                for(c = 0; c < GRIDCOLS; c++) {

                    if (c == 0) {

                        // if first col, init m to its value and continue to next run

                        m = (MarkType)cellValues[r, c];
                        continue;
                    }

                    // c > 0, we are not at the first column anymore
                    
                    if((MarkType)cellValues[r,c] == m) {

                        // if the element is the same as the last, continue to next col

                        continue;

                    }
                    else {

                        // if the element is different, this line is not a winner
                        // mark m as a nonplayer settable value and break out of checks for this row

                        m = MarkType.Free;
                        break;

                    }

                }

                // process if there is a win after checking through this row

                if (m == MarkType.Free) {

                    // if m is a nonplayer settable value, do nothing

                }
                else {

                    // m is set to a value that declares a clear winner

                    // mark the winner as player 1 if Xs won, or player 2 if Os won
                    winner = (m == MarkType.X) ? 1 : 2;

                    // return true that a winner was found
                    return true;

                }


            }
            #endregion

            #region investigate each col

            // iterate through cols
            for (c = 0; c < GRIDCOLS; c++) {


                // iterate through rows of current row
                for (r = 0; r < GRIDROWS; r++) {

                    if (r == 0) {

                        // if first row, init m to this r,c value and continue to next run

                        m = (MarkType)cellValues[r, c];
                        continue;
                    }

                    // r > 0, we are not at the first row anymore

                    if ((MarkType)cellValues[r, c] == m) {

                        // if the element is the same as the last, continue to next row

                        continue;

                    }
                    else {

                        // if the element is different, this line is not a winner
                        // mark m as a nonplayer settable value and break out of checks for this col

                        m = MarkType.Free;
                        break;

                    }

                }

                // process if there is a win after checking through this col

                if (m == MarkType.Free) {

                    // if m is a nonplayer settable value, do nothing .. moving on  to the next col

                }
                else {

                    // m is set to a value that declares a clear winner

                    // mark the winner as player 1 if Xs won, or player 2 if Os won
                    winner = (m == MarkType.X) ? 1 : 2;

                    // return true that a winner was found
                    return true;

                }


            }

            #endregion

            #region investigate topleft->botright diagonal

            // iterate through rows
            for (r = 0; r < GRIDROWS; r++) {

                
                if (r == 0) {

                    // if first check, set first marktype to r,r cell value

                    m = (MarkType)cellValues[r, r];
                }
                else {

                    // not first check, compare new cell value to old cell value

                    if(m == (MarkType)cellValues[r, r]) {

                        // cell value is same as last, continue on to next cell to check
                        continue;

                    }
                    else {

                        // cell value is different, drop out of diagnal check
                        m = MarkType.Free;
                        break;
                    }

                }

            }

            // process if there is a win after checking through this diag

            if (m == MarkType.Free) {

                // if m is a nonplayer settable value, do nothing, no winner yet .. moving on 

            }
            else {

                // m is set to a value that declares a clear winner

                // mark the winner as player 1 if Xs won, or player 2 if Os won
                winner = (m == MarkType.X) ? 1 : 2;

                // return true that a winner was found
                return true;

            }

            #endregion

            #region investigate botleft->topright diagonal

            // iterate through rows
            for (r = 0; r < GRIDROWS; r++) {

                if (r == 0) {

                    // if first check, set first marktype to r,r cell value

                    m = (MarkType)cellValues[r, GRIDCOLS - r - 1];
                }
                else {

                    // not first check, compare new cell value to old cell value

                    if (m == (MarkType)cellValues[r, GRIDCOLS - r - 1]) {

                        // cell value is same as last, it could be a win!.. continue on to check next cell
                        continue;

                    }
                    else {

                        // cell value is different, drop out of diagnal check
                        m = MarkType.Free;
                        break;
                    }

                }

            }

            // process if there is a win after checking through this diag

            if (m == MarkType.Free) {

                // if m is a nonplayer settable value, do nothing .. moving on  

            }
            else {

                // m is set to a value that declares a clear winner

                // mark the winner as player 1 if Xs won, or player 2 if Os won
                winner = (m == MarkType.X) ? 1 : 2;

                // return true that a winner was found
                return true;

            }


            #endregion

    
            // if no winner has been found, return false to the caller
            return false;
        }
    }


}
