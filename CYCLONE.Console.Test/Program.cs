
namespace CYCLONE.ConsoleApp.Test
{
    using ConsoleTables;
    using CYCLONE.API.JSONDecode;
    using CYCLONE.API.JSONDecode.Extension;
    using Simphony.Simulation;

    internal class Program
	{
		static void Main(string[] args)
		{
			string jsonString = """
				{
				  "type": "MAIN",
				  "processName": "MASONARY",
				  "lengthOfRun": 500,
				  "noOfCycles": 9,
				  "networkInput": [
				    {
				      "type": "QUEUE",
				      "label": 1,
				      "description": "Laborer Idle",
				      "numberToBeGenerated": 0,
				      "resourceInput": {
				        "type": "RESOURCE",
				        "noOfUnit": 1,
				        "description": "Laborer"
				      }
				    },
				    {
				      "type": "COMBI",
				      "label": 2,
				      "set": {
				        "type": "NST",
				        "distribution": {
				          "type": "beta",
				          "low": 0.001,
				          "high": 5,
				          "shape1": 2.6,
				          "shape2": 0.5
				        },
				        "category": "FIRST",
				        "par1": 1,
						"par2": 1
				      },
				      "description": "Resupply stack",
				      "followers": [
				        {
				          "type": "REF_QUEUE",
				          "value": 1
				        },
				        {
				          "type": "REF_QUEUE",
				          "value": 3
				        }
				      ],
				      "preceders": [
				        {
				          "type": "REF_QUEUE",
				          "value": 1
				        },
				        {
				          "type": "REF_QUEUE",
				          "value": 4
				        }
				      ]
				    },
				    {
				      "type": "QUEUE",
				      "label": 3,
				      "description": "Position Occupied",
				      "numberToBeGenerated": 0
				    },
				    {
				      "type": "QUEUE",
				      "label": 4,
				      "description": "Position Available",
				      "numberToBeGenerated": 0,
				      "resourceInput": {
				        "type": "RESOURCE",
				        "noOfUnit": 3,
				        "description": "Positions"
				      }
				    },
				    {
				      "type": "COMBI",
				      "label": 5,
				      "set": {
				        "type": "STATIONARY",
				        "distribution": {
				          "type": "deterministic",
				          "constant": 1
				        }
				      },
				      "description": "Mason removes packet",
				      "followers": [
				        {
				          "type": "REF_QUEUE",
				          "value": 4
				        },
				        {
				          "type": "REF_NORMAL",
				          "value": 6
				        }
				      ],
				      "preceders": [
				        {
				          "type": "REF_QUEUE",
				          "value": 3
				        },
				        {
				          "type": "REF_QUEUE",
				          "value": 7
				        }
				      ]
				    },
				    {
				      "type": "NORMAL",
				      "label": 6,
				      "set": {
				        "type": "STATIONARY",
				        "distribution": {
				          "type": "beta",
				          "low": 3,
				          "high": 10,
				          "shape1": 7,
				          "shape2": 2.2
				        }
				      },
				      "description": "Mason lays brick",
				      "followers": [
				        {
				          "type": "REF_FUNCTION",
				          "value": 8
				        }
				      ]
				    },
				    {
				      "type": "QUEUE",
				      "label": 7,
				      "description": "Mason waits resupply",
				      "numberToBeGenerated": 0,
				      "resourceInput": {
				        "type": "RESOURCE",
				        "noOfUnit": 3,
				        "description": "Masons"
				      }
				    },
				    {
				      "type": "FUNCTION_COUNTER",
				      "label": 8,
				      "quantity": 0,
				      "followers": [
				        {
				          "type": "REF_QUEUE",
				          "value": 7
				        }
				      ],
				      "description": ""
				    }
				  ]
				}
				""";

			var decoder = new Decoder(jsonString);
			var engine = new DiscreteEventEngine();
			var scenario = decoder.ToScenario(engine, debug: true);

			engine.InitializeEngine();
			var terminationReason = engine.Simulate(scenario).GetDescriptionAttribute();

            ConsoleTable consoleTables;
            Console.WriteLine("Intrinsic Results");
            consoleTables = new ConsoleTable("Element Name", "Mean", "Standard Deviation", "Minimum", "Maximum", "Current");
            foreach (var result in scenario.IntrinsicResults)
            {
                var intrinsicResult = result.Value;
                consoleTables.AddRow(
                    result.Key,
                    intrinsicResult.Mean,
                    intrinsicResult.StdDev,
                    intrinsicResult.Min,
                    intrinsicResult.Max,
                    intrinsicResult.Current);
            }

            consoleTables.Write();

            Console.WriteLine("Non-Intrinsic Results");
            consoleTables = new ConsoleTable("Element Name", "Mean", "Standard Deviation", "Observations", "Minimum", "Maximum");
            foreach (var result in scenario.NonIntrinsicResults)
            {
                var nonIntrinsicResult = result.Value;
                consoleTables.AddRow(
                    result.Key,
                    nonIntrinsicResult.Mean,
                    nonIntrinsicResult.StdDev,
                    nonIntrinsicResult.ObservationCount,
                    nonIntrinsicResult.Min,
                    nonIntrinsicResult.Max);
            }

            consoleTables.Write();

            Console.WriteLine("Counter Results");
            consoleTables = new ConsoleTable("Element Name", "Final Count", "Production Rate", "Average Inter-Arrival Time", "First Arrival", "Last Arrival");
            foreach (var result in scenario.CounterResults)
            {
                var counterResult = result.Value;
                consoleTables.AddRow(
                    result.Key,
                    counterResult.FinalCount,
                    counterResult.ProductionRate,
                    counterResult.AverageInterArrivalTime,
                    counterResult.FirstArrival,
                    counterResult.LastArrival);
            }

            consoleTables.Write();

            Console.WriteLine("Waiting File Results");
            consoleTables = new ConsoleTable("Element Name", "Average Length", "Standard Deviation", "Maximum Length", "Current Length", "Average Wait Time");
            foreach (var result in scenario.WaitingFileResults)
            {
                var waitingFileResult = result.Value;
                consoleTables.AddRow(
                    result.Key,
                    waitingFileResult.AverageLength,
                    waitingFileResult.StdDev,
                    waitingFileResult.MaxLength,
                    waitingFileResult.CurrentLength,
                    waitingFileResult.AvgWaitTime);
            }

            consoleTables.Write();

            Console.WriteLine("Termination Reason: " + terminationReason.ToString());
		}
	}
}