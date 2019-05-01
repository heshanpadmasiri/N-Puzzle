﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Puzzle
{
    abstract class Heruistic
    {
        private State goalState;
        public Heruistic(State goalState)
        {
            if(goalState == null)
            {
                throw new Exception("Goal state is null");
            }
            this.goalState = goalState;
        }
        abstract public float calculate(State state);
    }

    class Manhatten : Heruistic
    {
        public Manhatten(State goalState) : base(goalState)
        {
        }

        public override float calculate(State state)
        {
            throw new NotImplementedException();
        }
    }

    class Misplaced : Heruistic
    {
        public Misplaced(State goalState) : base(goalState)
        {
        }

        public override float calculate(State state)
        {
            throw new NotImplementedException();
        }
    }
    class State
    {
        private Heruistic heruistic;
        private List<List<String>> configuaration { get; set; }
        private float currentCost;
        private String lastMove { get; }
        public State(List<String> inputValues, String heruistic="none", State goalState=null)
        {
            setConfiguration(inputValues);
            if(String.Equals(heruistic.ToLower(), "manhatten"))
            {
                this.heruistic = new Manhatten(goalState);
            }
            else if (String.Equals(heruistic.ToLower(), "misplaced"))
            {
                this.heruistic = new Misplaced(goalState);
            } 
            else if (String.Equals(heruistic.ToLower(), "none"))
            {
                // used to create the goal state only
                this.heruistic = null;
            }
            else
            {
                throw new Exception("invalid heruist");
            }
            this.currentCost = 0;
            this.lastMove = null;
        }
        private State(List<List<String>> configuaration, Heruistic heruistic, float currentCost, String lastMove)
        {
            this.heruistic = heruistic;
            this.configuaration = configuaration;
            this.currentCost = currentCost;
            this.lastMove = lastMove;
        }

        private void setConfiguration(List<String> inputValues)
        {
            this.configuaration = new List<List<string>>();
            foreach(string line in inputValues){
                List<String> temp = line.Split('\t').ToList();
                configuaration.Add(temp);
            }
        }

        public float getCost()
        {           
            return this.currentCost + heruistic.calculate(this);
        }

        public List<State> getPossibleNextStates()
        {
            int J = this.configuaration.Count;
            int I = this.configuaration.ElementAt(0).Count;
            var possibleNextStates = new List<State>();
            for(int j = 0; j < J; j++)
            {
                for(int i = 0; i < I; i++)
                {
                    if(String.Equals(this.getValueAt(i,j), "-"))
                    {
                        var variations = this.getVariations(i, j);
                        foreach(State variation in variations)
                        {
                            possibleNextStates.Add(variation);
                        }
                    }
                }
            }
            return possibleNextStates;
        }

        private List<State> getVariations(int i, int j)
        {
            int J = this.configuaration.Count-1;
            int I = this.configuaration.ElementAt(0).Count-1;
            List<State> variations = new List<State>();
            if (j != 0)
            {
                // element at top can be moved down
                var tmp = this.getCopyOfConfiguration();
                String move = $"({this.getValueAt(i, j - 1)} , down)";
                swap(Tuple.Create(j, i), Tuple.Create(j - 1, i), tmp);
                variations.Add(new State(tmp, this.heruistic, this.currentCost+1, move));
            }
            if (j != J)
            {
                // element below can be moved up
                var tmp = this.getCopyOfConfiguration();
                String move = $"({this.getValueAt(i, j + 1)} , up)";
                swap(Tuple.Create(j, i), Tuple.Create(j + 1, i), tmp);
                variations.Add(new State(tmp, this.heruistic, this.currentCost+1, move));
            }
            if (i != 0)
            {
                // element to right can be moved left
                var tmp = this.getCopyOfConfiguration();
                String move = $"({this.getValueAt(i-1, j)} , right)";
                swap(Tuple.Create(j, i), Tuple.Create(j, i-1), tmp);
                variations.Add(new State(tmp, this.heruistic, this.currentCost + 1, move));
            }
            if (i != I)
            {
                // element to left can be moved right
                var tmp = this.getCopyOfConfiguration();
                String move = $"({this.getValueAt(i+1, j)} , left)";
                swap(Tuple.Create(j, i), Tuple.Create(j, i+1), tmp);
                variations.Add(new State(tmp, this.heruistic, this.currentCost + 1, move));
            }
            return variations;
        }

        private void swap(Tuple<int,int> a, Tuple<int,int> b, List<List<String>> configuaration)
        {
            var tmp = configuaration[a.Item1][a.Item2];
            configuaration[a.Item1][a.Item2] = configuaration[b.Item1][b.Item2];
            configuaration[b.Item1][b.Item2] = tmp;
        }

        public string getValueAt(int i, int j)
        {
            return this.configuaration.ElementAt(j)[i];
        }

        private List<List<String>> getCopyOfConfiguration()
        {
            var copy = new List<List<String>>();
            foreach(List<String> row in this.configuaration){
                var tmp = new List<String>();
                foreach(string val in row)
                {
                    tmp.Add(val);
                }
                copy.Add(tmp);
            }
            return copy;
        }

        public override bool Equals(object obj)
        {
            // if dimensions are not equal behavior is unpredictable
            var other = (State)obj;
            var otherConfig = other.configuaration;
            if (otherConfig.Count != this.configuaration.Count)
            {
                return false;
            }
            for(int j = 0; j < otherConfig.Count; j++)
            {
                var thisRow = this.configuaration.ElementAt(j);
                var otherRow = otherConfig.ElementAt(j);
                if (thisRow.Count != otherRow.Count)
                {
                    return false;
                }
                for(int i = 0; i < thisRow.Count; i++)
                {
                    if(!String.Equals(thisRow.ElementAt(i), otherRow.ElementAt(i)))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            return 2062130366 + EqualityComparer<List<List<string>>>.Default.GetHashCode(configuaration);
        }
    }
    class Program
    {
        private static List<String> getInputs(string fileName)
        {
            // use to get input as line array
            var filePath = AppDomain.CurrentDomain.BaseDirectory + '\\' + fileName;
            var file = new System.IO.StreamReader(filePath);
            string line;
            List<String> input = new List<string>();
            while ((line = file.ReadLine()) != null)
            {
                input.Add(line);
            }
            file.Close();
            return input;
        }

        private static State getGoalState(string goalCofigFileName)
        {
            var input = getInputs(goalCofigFileName);
            return new State(input);
        }

        private static Tuple<State, State> createInitialState(string startConfigFileName, State goalState)
        {
            // returns the Manhatten start state and misplaced start state
            var input = getInputs(startConfigFileName);
            State manhatten = new State(input, "manhatten", goalState);
            State misplaced = new State(input, "misplaced", goalState);
            
            return Tuple.Create(manhatten, misplaced);
        }

        static void solve(State startingState, State goalState)
        {
            var newStates = startingState.getPossibleNextStates();
            return;
        }
        static void Main(string[] args)
        {
            String startConfigFileName = args[0];
            String goalConfigFileName = args[1];
            var goalState = getGoalState(goalConfigFileName);
            var initialStates = createInitialState(startConfigFileName, goalState);
            var manhattenInitialState = initialStates.Item1;
            var misplacedInitialState = initialStates.Item2;
            solve(manhattenInitialState, goalState);
        }
    }
}
