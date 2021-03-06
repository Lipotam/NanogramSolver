﻿using System;
using System.Linq;
using NonogramSolver.Models;

namespace NonogramSolver.Core
{
    class NonogramResolver
    {
        public NonogramResolver(CrosswordData crosswordData)
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

            crosswordInitialData.FieldCells = workingData.Matrix;
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

                this.workingData.SaveColumn(i, MakeSearchInLine(column, numbers));

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

                this.workingData.SaveLine(i, MakeSearchInLine(line, numbers));
            }
        }


        private static CellState[] MakeSearchInLine(CellState[] elementsInMatrix, PanelLine numbers)
        {
            StateGeneratorModified mgenerator = new StateGeneratorModified();

            CellState[] result = new CellState[elementsInMatrix.Length];
            for (int i = 0; i < elementsInMatrix.Length; i++)
            {
                result[i] = elementsInMatrix[i];
            }

            foreach (CellState[] generatorState in mgenerator.GetStatesList(numbers, elementsInMatrix))
            {
                if (IsPossibleState(elementsInMatrix, generatorState))
                {
                    result = MergeStatesForSearch(result, generatorState);
                }
            }

            for (int i = 0; i < result.Count(); i++)
            {
                if (result[i] == CellState.None)
                {
                    result[i] = CellState.Undefined;
                }
            }

            if (!IsPossibleState(elementsInMatrix, result))
            {
                throw new Exception("found elements crash the matrix");
            }
            return result;
        }

        private static bool IsPossibleState(CellState[] currentConstState, CellState[] possibleState)
        {
            for (int i = 0; i < currentConstState.Count(); i++)
            {
                if (currentConstState[i] == CellState.Undefined)
                {
                    continue;
                }
                if (currentConstState[i] == CellState.Filled && possibleState[i] == CellState.Empty)
                {
                    return false;
                }
                if (currentConstState[i] == CellState.Empty && possibleState[i] == CellState.Filled)
                {
                    return false;
                }
                if (currentConstState[i] == CellState.Undefined)
                {
                    throw new Exception("matrix has impossible state");
                }
            }
            return true;
        }

        private static CellState[] MergeStatesForSearch(CellState[] currentStates, CellState[] possibleState)
        {
            for (int i = 0; i < currentStates.Count(); i++)
            {
                if (currentStates[i] == CellState.None)
                {
                    continue;
                }
                if (currentStates[i] == CellState.Filled && possibleState[i] == CellState.Empty)
                {
                    currentStates[i] = CellState.None;
                }
                if (currentStates[i] == CellState.Empty && possibleState[i] == CellState.Filled)
                {
                    currentStates[i] = CellState.None;
                }
                if (currentStates[i] == CellState.Undefined)
                {
                    currentStates[i] = possibleState[i];
                }
            }

            return currentStates;
        }
    }
}
