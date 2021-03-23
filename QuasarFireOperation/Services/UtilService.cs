using QuasarFireOperation.Models;
using System;

namespace QuasarFireOperation.Services
{
    public static class UtilService
    {
        public static PositionDTO GetLocationByTrilateration(InformationMessageSatelliteDTO satelliteOne, InformationMessageSatelliteDTO satelliteTwo, InformationMessageSatelliteDTO satelliteThree)
        {
            PositionDTO shipLocation = new PositionDTO();

            double A = 2 * satelliteTwo.x_position - 2 * satelliteOne.x_position;
            double B = 2 * satelliteTwo.y_position - 2 * satelliteOne.y_position;
            double C = (Math.Pow(satelliteOne.distance, 2.0) - Math.Pow(satelliteTwo.distance, 2.0) - Math.Pow(satelliteOne.x_position, 2.0) + Math.Pow(satelliteTwo.x_position, 2.0) - Math.Pow(satelliteOne.y_position, 2.0) + Math.Pow(satelliteTwo.y_position, 2.0));

            double D = 2 * satelliteThree.x_position - 2 * satelliteTwo.x_position;
            double E = 2 * satelliteThree.y_position - 2 * satelliteTwo.y_position;
            double F = (Math.Pow(satelliteTwo.distance, 2) - Math.Pow(satelliteThree.distance, 2) - Math.Pow(satelliteTwo.x_position, 2) + Math.Pow(satelliteThree.x_position, 2) - Math.Pow(satelliteTwo.y_position, 2) + Math.Pow(satelliteThree.y_position, 2));

            shipLocation.x = Math.Round(((C * E - F * B) / (E * A - B * D)),2);
            shipLocation.y = Math.Round(((C * D - A * F) / (B * D - A * E)),2);

            return shipLocation;
        }

        public static double SubtractEquation(PositionDTO positionOne, float distanceOne, PositionDTO positionTwo, float distanceTwo)
        {
            return Math.Pow(distanceOne, 2.0) - Math.Pow(distanceTwo, 2.0) - Math.Pow(positionOne.x, 2.0) + Math.Pow(positionTwo.x, 2.0) - Math.Pow(positionOne.y, 2.0) + Math.Pow(positionTwo.y, 2.0);
        }

        public static double DistanceCalculate(PositionDTO positionOne, PositionDTO positionTwo)
        {
            double distance = Math.Sqrt(Math.Pow(positionTwo.x - (positionOne.x), 2) + Math.Pow(positionTwo.y - (positionOne.y), 2));

            return distance;
        }               

        public static string[] WordsCompare(string[] msgOne, string[] msgTwo)
        {
            string[] msgNew = new string[msgOne.Length];

            if (msgOne.Length == msgTwo.Length)
            {
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
            }

            return msgNew;
        }

        public static string FormatText(string[] messages)
        {
            string text = String.Empty;

            foreach (string word in messages)
            {
                if (!String.IsNullOrEmpty(word))
                    text += word + " ";
                else
                    return String.Empty;
            }

            return text.Trim();
        }

        public static string ArrayToString(string[] arrayString)
        {
            string valueReturn = String.Empty;

            for (int i = 0; i < arrayString.Length; i++)
            {
                string value = arrayString[i];

                if (i+1 == arrayString.Length)
                    valueReturn += value;
                else
                    valueReturn += value+",";
            }

            return valueReturn;
        }
    }
}
