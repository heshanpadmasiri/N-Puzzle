using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Puzzle
{
    abstract class Heruistic
    {
        abstract public float calculate(State state);
    }

    class Manhatten : Heruistic
    {
        public override float calculate(State state)
        {
            throw new NotImplementedException();
        }
    }

    class Misplaced : Heruistic
    {
        public override float calculate(State state)
        {
            throw new NotImplementedException();
        }
    }
    class State
    {
        private Heruistic heruistic;
        private List<List<String>> configuaration;
        private float currentCost;
        private String lastMove { get; }
        public State(List<String> inputValues, String heruistic)
        {
            setConfiguration(inputValues);
            if(String.Equals(heruistic.ToLower(), "manhatten"))
            {
                this.heruistic = new Manhatten();
            } else if (String.Equals(heruistic.ToLower(), "misplaced"))
            {
                this.heruistic = new Misplaced();
            } else
            {
                throw new Exception("invalid heruist");
            }
            this.currentCost = 0;
            this.lastMove = null;
        }
        private State(List<List<String>> configuaration, Heruistic heruistic, float currentCost, String LastMove)
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
        
    }
    class Program
    {
        static void Main(string[] args)
        {
            String startConfigFileName = args[0];
            String goalConfigFileName = args[1];
            System.Console.WriteLine(startConfigFileName);
            System.Console.WriteLine(goalConfigFileName);
        }
    }
}
