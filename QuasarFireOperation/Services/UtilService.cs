using QuasarFireOperation.Models;
using System;

namespace QuasarFireOperation.Services
{
    public static class UtilService
    {
        public static PositionDTO getLocationByTrilateration(PositionDTO coordinateOne, double distanceOne, PositionDTO coordinateTwo, double distanceTwo, PositionDTO coordinateThree, double distanceThree)
        {
            PositionDTO shipLocation = new PositionDTO();

            double A = 2 * coordinateTwo.x - 2 * coordinateOne.x;
            double B = 2 * coordinateTwo.y - 2 * coordinateOne.y;
            double C = (Math.Pow(distanceOne, 2.0) - Math.Pow(distanceTwo, 2.0) - Math.Pow(coordinateOne.x, 2.0) + Math.Pow(coordinateTwo.x, 2.0) - Math.Pow(coordinateOne.y, 2.0) + Math.Pow(coordinateTwo.y, 2.0));

            double D = 2 * coordinateThree.x - 2 * coordinateTwo.x;
            double E = 2 * coordinateThree.y - 2 * coordinateTwo.y;
            double F = (Math.Pow(distanceTwo, 2) - Math.Pow(distanceThree, 2) - Math.Pow(coordinateTwo.x, 2) + Math.Pow(coordinateThree.x, 2) - Math.Pow(coordinateTwo.y, 2) + Math.Pow(coordinateThree.y, 2));

            shipLocation.x = (C * E - F * B) / (E * A - B * D);
            shipLocation.y = (C * D - A * F) / (B * D - A * E);

            return shipLocation;
        }

        public static double subtractEquation(PositionDTO positionOne, float distanceOne, PositionDTO positionTwo, float distanceTwo)
        {
            return Math.Pow(distanceOne, 2.0) - Math.Pow(distanceTwo, 2.0) - Math.Pow(positionOne.x, 2.0) + Math.Pow(positionTwo.x, 2.0) - Math.Pow(positionOne.y, 2.0) + Math.Pow(positionTwo.y, 2.0);
        }

        public static double distanceCalculate(PositionDTO positionOne, PositionDTO positionTwo)
        {
            double distance = Math.Sqrt(Math.Pow(positionTwo.x - (positionOne.x), 2) + Math.Pow(positionTwo.y - (positionOne.y), 2));

            return distance;
        }               

        public static string[] WordsCompare(string[] msgOne, string[] msgTwo)
        {
            int arrayLength = msgOne.Length > msgTwo.Length ? msgOne.Length : msgTwo.Length; //tomamos el tamaño mas grande del array a comparar

            //suponemos que el tamañano de los array son iguales
            string[] msgNew = new string[arrayLength];

            for (int i = 0; i < msgOne.Length; i++)
            {
                if (!String.IsNullOrEmpty(msgOne[i]))
                {
                    msgNew[i] = msgOne[i].Trim();
                }
                else
                {
                    msgNew[i] = msgTwo[i].Trim();
                }
            }

            return msgNew;
        }

        public static string FormatText(string[] messages)
        {
            string text = String.Empty;

            foreach (string word in messages)
            {
                text += word + " "; //
            }

            return text.Trim();
        }

        public static string arrayToString(string[] arrayString)
        {
            string valueReturn = String.Empty;

            foreach (string value in arrayString)
            {
                valueReturn += String.IsNullOrEmpty(valueReturn) ? value : "," + value;
            }

            return valueReturn;
        }
    }
}
