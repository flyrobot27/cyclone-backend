
namespace CYCLONE.ConsoleApp.Test
{
    using ConsoleTables;
    using CYCLONE.API.JSONDecode;
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
				        "par1": 0
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
			var terminationReason = engine.Simulate(scenario);

            ConsoleTable consoleTables;
            Console.WriteLine("Intrinsic Results");
            consoleTables = new ConsoleTable("Element Name", "Mean", "Standard Deviation", "Minimum", "Maximum", "Current");
            foreach (var result in scenario.IntrinsicResults)
            {
                var intrinsicResult = result.Value;
                consoleTables.AddRow(
                    result.Key,
                    intrinsicResult.Mean.ToString("N3"),
                    intrinsicResult.StdDev.ToString("N3"),
                    intrinsicResult.Min.ToString("N3"),
                    intrinsicResult.Max.ToString("N3"),
                    intrinsicResult.Current.ToString("N3"));
            }

            consoleTables.Write();

            Console.WriteLine("Non-Intrinsic Results");
            consoleTables = new ConsoleTable("Element Name", "Mean", "Standard Deviation", "Observations", "Minimum", "Maximum");
            foreach (var result in scenario.NonIntrinsicResults)
            {
                var nonIntrinsicResult = result.Value;
                consoleTables.AddRow(
                    result.Key,
                    nonIntrinsicResult.Mean.ToString("N3"),
                    nonIntrinsicResult.StdDev.ToString("N3"),
                    nonIntrinsicResult.ObservationCount.ToString("N3"),
                    nonIntrinsicResult.Min.ToString("N3"),
                    nonIntrinsicResult.Max.ToString("N3"));
            }

            consoleTables.Write();

            Console.WriteLine("Counter Results");
            consoleTables = new ConsoleTable("Element Name", "Final Count", "Production Rate", "Average Inter-Arrival Time", "First Arrival", "Last Arrival");
            foreach (var result in scenario.CounterResults)
            {
                var counterResult = result.Value;
                consoleTables.AddRow(
                    result.Key,
                    counterResult.FinalCount.ToString("N3"),
                    counterResult.ProductionRate.ToString("N3"),
                    counterResult.AverageInterArrivalTime.ToString("N3"),
                    counterResult.FirstArrival.ToString("N3"),
                    counterResult.LastArrival.ToString("N3"));
            }

            consoleTables.Write();

            Console.WriteLine("Waiting File Results");
            consoleTables = new ConsoleTable("Element Name", "Average Length", "Standard Deviation", "Maximum Length", "Current Length", "Average Wait Time");
            foreach (var result in scenario.WaitingFileResults)
            {
                var waitingFileResult = result.Value;
                consoleTables.AddRow(
                    result.Key,
                    waitingFileResult.AverageLength.ToString("N3"),
                    waitingFileResult.StdDev.ToString("N3"),
                    waitingFileResult.MaxLength.ToString("N3"),
                    waitingFileResult.CurrentLength.ToString("N3"),
                    waitingFileResult.AvgWaitTime.ToString("N3"));
            }

            consoleTables.Write();

            Console.WriteLine("Termination Reason: " + terminationReason.ToString());
		}
	}
}