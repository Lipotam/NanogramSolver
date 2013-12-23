using System.Linq;
using NonogramSolver.Models;

namespace NonogramSolver.Core
{
    class NonogramSolver
    {
        public NonogramSolver(CrosswordData crosswordData)
        {
            this.crosswordInitialData = crosswordData;
        }

        private readonly CrosswordData crosswordInitialData;
        private SolverModel workingData;

        public void StartResolving()
        {
            this.workingData = new SolverModel(this.crosswordInitialData);
            this.workingData.SetChangesForFirstSearch();

            while (this.workingData.HasChanges())
            {
                this.LineSearchLoop();
                this.workingData.ResetLineChangedMarkers();
                this.ColumnSearchLoop();
                this.workingData.ResetColumnChangedMarkers();
            }
        }

        private void ColumnSearchLoop()
        {
            bool[] changes = this.workingData.ColumnsChanged;

            for (int i = 0; i < this.workingData.FieldWidth; i++)
            {
                if (!changes[i])
                {
                    continue;
                }

                CellState[] column = this.workingData.GetColumn(i);
                PanelLine numbers = this.crosswordInitialData.TopPanelLines[i];

                this.workingData.SaveColumn(i, this.MakeSearchInLine(column, numbers));

            }
        }

        private void LineSearchLoop()
        {
            bool[] changes = this.workingData.LinesChanged;

            for (int i = 0; i < this.workingData.FieldHeight; i++)
            {
                if (!changes[i])
                {
                    continue;
                }

                CellState[] line = this.workingData.GetLine(i);
                PanelLine numbers = this.crosswordInitialData.LeftPanelLines[i];

                this.workingData.SaveColumn(i, this.MakeSearchInLine(line, numbers));
            }
        }


        private CellState[] MakeSearchInLine(CellState[] elements, PanelLine numbers)
        {
            StatesGenerator generator = new StatesGenerator(elements, numbers);
            CellState[] result = elements;

            while (true)
            {
                CellState[] generatorState = generator.GetNextState();
                if (generatorState == null)
                {
                    break;
                }
                result = this.MergeStatesForSearch(result, generatorState);
            }

            for (int i = 0; i < result.Count(); i++)
            {
                if (result[i] == CellState.None)
                {
                    result[i] = CellState.Undefined;
                }
            }
            return result;
        }

        private CellState[] MergeStatesForSearch(CellState[] currentStates, CellState[] possibleStates)
        {
            for (int i = 0; i < currentStates.Count(); i++)
            {
                if (currentStates[i] == CellState.None)
                {
                    continue;
                }
                if (currentStates[i] == CellState.Filled && possibleStates[i] == CellState.Empty)
                {
                    currentStates[i] = CellState.None;
                }
                if (currentStates[i] == CellState.Empty && possibleStates[i] == CellState.Filled)
                {
                    currentStates[i] = CellState.None;
                }
                if (currentStates[i] == CellState.Undefined)
                {
                    currentStates[i] = possibleStates[i];
                }
            }

            return currentStates;
        }
    }
}
