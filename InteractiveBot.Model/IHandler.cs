using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractiveBot.Model
{
    public interface IHandler
    {
        Task<QnAMakerResult> Handle(string input);
    }

    [Serializable]
    public class QnAMakerResult
    {
        public string Answer { get; set; }
        
        public double Score { get; set; }
    }
}
