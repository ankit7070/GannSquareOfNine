using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GannLibrary
{
    public class Gann
    {
        private const double buyMultiplier = 0.9995;
        private const double sellMultiplier = 1.0005;
        int decimalPlaces;
        double cmp;
        double firstValDown;
        double secondValDown;
        double firstValUp;
        double secondValUp;
        double[,] gannInternalValues = new double[7, 7];

        #region BuyValues
        public double BuyAt { get; set; }
        public double BuyTargetOne
        { get { return RoundDown(this.ResistanceOne * buyMultiplier); } }
        public double BuyTargetTwo
        { get { return RoundDown(this.ResistanceTwo * buyMultiplier); } }
        public double BuyTargetThree
        { get { return RoundDown(this.ResistanceThree * buyMultiplier); } }
        public double BuyTargetFour
        { get { return RoundDown(this.ResistanceFour * buyMultiplier); } }
        public double BuyTargetFive
        { get { return RoundDown(this.ResistanceFive * buyMultiplier); } }
        #endregion

        #region SellValues
        public double SellAt { get; set; }
        public double SellTargetOne
        { get { return RoundDown(this.SupportOne * sellMultiplier); } }
        public double SellTargetTwo
        { get { return RoundDown(this.SupportTwo * sellMultiplier); } }
        public double SellTargetThree
        { get { return RoundDown(this.SupportThree * sellMultiplier); } }
        public double SellTargetFour
        { get { return RoundDown(this.SupportFour * sellMultiplier); } }
        public double SellTargetFive
        { get { return RoundDown(this.SupportFive * sellMultiplier); } }
        #endregion

        #region ResistanceValues
        private double ResistanceOne;
        private double ResistanceTwo;
        private double ResistanceThree;
        private double ResistanceFour;
        private double ResistanceFive;
        #endregion

        #region SupportValues
        private double SupportOne;
        private double SupportTwo;
        private double SupportThree;
        private double SupportFour;
        private double SupportFive;
        #endregion
        public Gann(double currentMarketPrice, int decimalPlacesForRounding = -1)
        {
            this.decimalPlaces = decimalPlacesForRounding;
            cmp = currentMarketPrice;
            double squareRoot = Math.Sqrt(cmp);
            firstValDown = Math.Floor(squareRoot);
            secondValDown = firstValDown - 1;
            firstValUp = Math.Ceiling(squareRoot);
            secondValUp = firstValUp + 1;

            CalculateGann();
        }

        public double RoundDown(double number)
        {
            if (decimalPlaces != -1)
                return Math.Floor(number * Math.Pow(10, decimalPlaces)) / Math.Pow(10, decimalPlaces);
            else
                return number;
        }

        private void CalculateGann()
        {
            UpdateFirstDiagonal();
            UpdateSecondDiagonal();
            UpdateMiddleVerticalRow();
            UpdateMiddleHorizontalRow();
            UpdateCalculatedValues();

            CalculateBuyAt();
            CalculateSellAt();
            CalculateResistance();
            CalculateSupport();
        }

        private void CalculateBuyAt()
        {
            if (gannInternalValues[4, 1] != 0)
                this.BuyAt = gannInternalValues[3, 1];
            else if (gannInternalValues[2, 1] != 0)
                this.BuyAt = gannInternalValues[1, 1];
            else if (gannInternalValues[1, 2] != 0)
                this.BuyAt = gannInternalValues[1, 3];
            else if (gannInternalValues[1, 4] != 0)
                this.BuyAt = gannInternalValues[1, 5];
            else if (gannInternalValues[2, 5] != 0)
                this.BuyAt = gannInternalValues[3, 5];
            else if (gannInternalValues[4, 5] != 0)
                this.BuyAt = gannInternalValues[5, 5];
            else if (gannInternalValues[5, 4] != 0)
                this.BuyAt = gannInternalValues[5, 3];
            else if (gannInternalValues[5, 2] != 0)
                this.BuyAt = gannInternalValues[5, 1];
            else if (gannInternalValues[5, 0] != 0)
                this.BuyAt = gannInternalValues[3, 0];
            else if (gannInternalValues[2, 0] != 0)
                this.BuyAt = gannInternalValues[0, 0];
            else if (gannInternalValues[0, 1] != 0)
                this.BuyAt = gannInternalValues[0, 3];
            else if (gannInternalValues[0, 4] != 0)
                this.BuyAt = gannInternalValues[0, 6];
            else if (gannInternalValues[1, 6] != 0)
                this.BuyAt = gannInternalValues[3, 6];
            else if (gannInternalValues[4, 6] != 0)
                this.BuyAt = gannInternalValues[6, 6];
            else if (gannInternalValues[6, 5] != 0)
                this.BuyAt = gannInternalValues[6, 3];
            else if (gannInternalValues[6, 2] != 0)
                this.BuyAt = gannInternalValues[6, 0];

            this.BuyAt = RoundDown(this.BuyAt);          
        }

        private void CalculateSellAt()
        {
            if (gannInternalValues[4, 1] != 0)
                this.SellAt = gannInternalValues[4, 2];
            else if (gannInternalValues[2, 1] != 0)
                this.SellAt = gannInternalValues[2, 2];
            else if (gannInternalValues[1, 2] != 0)
                this.SellAt = gannInternalValues[1, 1];
            else if (gannInternalValues[1, 4] != 0)
                this.SellAt = gannInternalValues[1, 3];
            else if (gannInternalValues[2, 5] != 0)
                this.SellAt = gannInternalValues[1, 5];
            else if (gannInternalValues[4, 5] != 0)
                this.SellAt = gannInternalValues[3, 5];
            else if (gannInternalValues[5, 4] != 0)
                this.SellAt = gannInternalValues[5, 5];
            else if (gannInternalValues[5, 2] != 0)
                this.SellAt = gannInternalValues[5, 3];
            else if (gannInternalValues[5, 0] != 0)
                this.SellAt = gannInternalValues[5, 1];
            else if (gannInternalValues[2, 0] != 0)
                this.SellAt = gannInternalValues[3, 0];
            else if (gannInternalValues[0, 1] != 0)
                this.SellAt = gannInternalValues[0, 0];
            else if (gannInternalValues[0, 4] != 0)
                this.SellAt = gannInternalValues[0, 3];
            else if (gannInternalValues[1, 6] != 0)
                this.SellAt = gannInternalValues[0, 6];
            else if (gannInternalValues[4, 6] != 0)
                this.SellAt = gannInternalValues[3, 6];
            else if (gannInternalValues[6, 5] != 0)
                this.SellAt = gannInternalValues[6, 6];
            else if (gannInternalValues[6, 2] != 0)
                this.SellAt = gannInternalValues[6, 3];

            this.SellAt = RoundDown(this.SellAt);
        }

        private void CalculateResistance()
        {
            CalculateR1();
            CalculateR2();
            CalculateR3();
            CalculateR4();
            CalculateR5();
        }

        private void CalculateR1()
        {
            if (gannInternalValues[4, 1] != 0)
                this.ResistanceOne = gannInternalValues[1, 1];
            else if (gannInternalValues[2, 1] != 0)
                this.ResistanceOne = gannInternalValues[1, 3];
            else if (gannInternalValues[1, 2] != 0)
                this.ResistanceOne = gannInternalValues[1, 5];
            else if (gannInternalValues[1, 4] != 0)
                this.ResistanceOne = gannInternalValues[3, 5];
            else if (gannInternalValues[2, 5] != 0)
                this.ResistanceOne = gannInternalValues[5, 5];
            else if (gannInternalValues[4, 5] != 0)
                this.ResistanceOne = gannInternalValues[5, 3];
            else if (gannInternalValues[5, 4] != 0)
                this.ResistanceOne = gannInternalValues[5, 1];
            else if (gannInternalValues[5, 2] != 0)
                this.ResistanceOne = gannInternalValues[3, 0];
            else if (gannInternalValues[5, 0] != 0)
                this.ResistanceOne = gannInternalValues[0, 0];
            else if (gannInternalValues[2, 0] != 0)
                this.ResistanceOne = gannInternalValues[0, 3];
            else if (gannInternalValues[0, 1] != 0)
                this.ResistanceOne = gannInternalValues[0, 6];
            else if (gannInternalValues[0, 4] != 0)
                this.ResistanceOne = gannInternalValues[3, 6];
            else if (gannInternalValues[1, 6] != 0)
                this.ResistanceOne = gannInternalValues[6, 6];
            else if (gannInternalValues[4, 6] != 0)
                this.ResistanceOne = gannInternalValues[6, 3];
            else if (gannInternalValues[6, 5] != 0)
                this.ResistanceOne = gannInternalValues[6, 0];
            else if (gannInternalValues[6, 2] != 0)
                this.ResistanceOne = gannInternalValues[6, 0];
        }

        private void CalculateR2()
        {
            if (gannInternalValues[4, 1] != 0)
                this.ResistanceTwo = gannInternalValues[1, 3];
            else if (gannInternalValues[2, 1] != 0)
                this.ResistanceTwo = gannInternalValues[1, 5];
            else if (gannInternalValues[1, 2] != 0)
                this.ResistanceTwo = gannInternalValues[3, 5];
            else if (gannInternalValues[1, 4] != 0)
                this.ResistanceTwo = gannInternalValues[5, 5];
            else if (gannInternalValues[2, 5] != 0)
                this.ResistanceTwo = gannInternalValues[5, 3];
            else if (gannInternalValues[4, 5] != 0)
                this.ResistanceTwo = gannInternalValues[5, 1];
            else if (gannInternalValues[5, 4] != 0)
                this.ResistanceTwo = gannInternalValues[3, 0];
            else if (gannInternalValues[5, 2] != 0)
                this.ResistanceTwo = gannInternalValues[0, 0];
            else if (gannInternalValues[5, 0] != 0)
                this.ResistanceTwo = gannInternalValues[0, 3];
            else if (gannInternalValues[2, 0] != 0)
                this.ResistanceTwo = gannInternalValues[0, 6];
            else if (gannInternalValues[0, 1] != 0)
                this.ResistanceTwo = gannInternalValues[3, 6];
            else if (gannInternalValues[0, 4] != 0)
                this.ResistanceTwo = gannInternalValues[6, 6];
            else if (gannInternalValues[1, 6] != 0)
                this.ResistanceTwo = gannInternalValues[6, 3];
            else if (gannInternalValues[4, 6] != 0)
                this.ResistanceTwo = gannInternalValues[6, 0];
            else if (gannInternalValues[6, 5] != 0)
                this.ResistanceTwo = gannInternalValues[6, 0];
            else if (gannInternalValues[6, 2] != 0)
                this.ResistanceTwo = gannInternalValues[6, 0];
        }

        private void CalculateR3()
        {
            if (gannInternalValues[4, 1] != 0)
                this.ResistanceThree = gannInternalValues[1, 5];
            else if (gannInternalValues[2, 1] != 0)
                this.ResistanceThree = gannInternalValues[3, 5];
            else if (gannInternalValues[1, 2] != 0)
                this.ResistanceThree = gannInternalValues[5, 5];
            else if (gannInternalValues[1, 4] != 0)
                this.ResistanceThree = gannInternalValues[5, 3];
            else if (gannInternalValues[2, 5] != 0)
                this.ResistanceThree = gannInternalValues[5, 1];
            else if (gannInternalValues[4, 5] != 0)
                this.ResistanceThree = gannInternalValues[3, 0];
            else if (gannInternalValues[5, 4] != 0)
                this.ResistanceThree = gannInternalValues[0, 0];
            else if (gannInternalValues[5, 2] != 0)
                this.ResistanceThree = gannInternalValues[0, 3];
            else if (gannInternalValues[5, 0] != 0)
                this.ResistanceThree = gannInternalValues[0, 6];
            else if (gannInternalValues[2, 0] != 0)
                this.ResistanceThree = gannInternalValues[3, 6];
            else if (gannInternalValues[0, 1] != 0)
                this.ResistanceThree = gannInternalValues[6, 6];
            else if (gannInternalValues[0, 4] != 0)
                this.ResistanceThree = gannInternalValues[6, 3];
            else if (gannInternalValues[1, 6] != 0)
                this.ResistanceThree = gannInternalValues[6, 0];
            else if (gannInternalValues[4, 6] != 0)
                this.ResistanceThree = gannInternalValues[6, 0];
            else if (gannInternalValues[6, 5] != 0)
                this.ResistanceThree = gannInternalValues[6, 0];
            else if (gannInternalValues[6, 2] != 0)
                this.ResistanceThree = gannInternalValues[6, 0];
        }

        private void CalculateR4()
        {
            if (gannInternalValues[4, 1] != 0)
                this.ResistanceFour = gannInternalValues[3, 5];
            else if (gannInternalValues[2, 1] != 0)
                this.ResistanceFour = gannInternalValues[5, 5];
            else if (gannInternalValues[1, 2] != 0)
                this.ResistanceFour = gannInternalValues[5, 3];
            else if (gannInternalValues[1, 4] != 0)
                this.ResistanceFour = gannInternalValues[5, 1];
            else if (gannInternalValues[2, 5] != 0)
                this.ResistanceFour = gannInternalValues[3, 0];
            else if (gannInternalValues[4, 5] != 0)
                this.ResistanceFour = gannInternalValues[0, 0];
            else if (gannInternalValues[5, 4] != 0)
                this.ResistanceFour = gannInternalValues[0, 3];
            else if (gannInternalValues[5, 2] != 0)
                this.ResistanceFour = gannInternalValues[0, 6];
            else if (gannInternalValues[5, 0] != 0)
                this.ResistanceFour = gannInternalValues[3, 6];
            else if (gannInternalValues[2, 0] != 0)
                this.ResistanceFour = gannInternalValues[6, 6];
            else if (gannInternalValues[0, 1] != 0)
                this.ResistanceFour = gannInternalValues[6, 3];
            else if (gannInternalValues[0, 4] != 0)
                this.ResistanceFour = gannInternalValues[6, 0];
            else if (gannInternalValues[1, 6] != 0)
                this.ResistanceFour = gannInternalValues[6, 0];
            else if (gannInternalValues[4, 6] != 0)
                this.ResistanceFour = gannInternalValues[6, 0];
            else if (gannInternalValues[6, 5] != 0)
                this.ResistanceFour = gannInternalValues[6, 0];
            else if (gannInternalValues[6, 2] != 0)
                this.ResistanceFour = gannInternalValues[6, 0];
        }

        private void CalculateR5()
        {
            if (gannInternalValues[4, 1] != 0)
                this.ResistanceFive = gannInternalValues[5, 5];
            else if (gannInternalValues[2, 1] != 0)
                this.ResistanceFive = gannInternalValues[5, 3];
            else if (gannInternalValues[1, 2] != 0)
                this.ResistanceFive = gannInternalValues[5, 1];
            else if (gannInternalValues[1, 4] != 0)
                this.ResistanceFive = gannInternalValues[3, 0];
            else if (gannInternalValues[2, 5] != 0)
                this.ResistanceFive = gannInternalValues[0, 0];
            else if (gannInternalValues[4, 5] != 0)
                this.ResistanceFive = gannInternalValues[0, 3];
            else if (gannInternalValues[5, 4] != 0)
                this.ResistanceFive = gannInternalValues[0, 6];
            else if (gannInternalValues[5, 2] != 0)
                this.ResistanceFive = gannInternalValues[3, 6];
            else if (gannInternalValues[5, 0] != 0)
                this.ResistanceFive = gannInternalValues[6, 6];
            else if (gannInternalValues[2, 0] != 0)
                this.ResistanceFive = gannInternalValues[6, 3];
            else if (gannInternalValues[0, 1] != 0)
                this.ResistanceFive = gannInternalValues[6, 0];
            else if (gannInternalValues[0, 4] != 0)
                this.ResistanceFive = gannInternalValues[6, 0];
            else if (gannInternalValues[1, 6] != 0)
                this.ResistanceFive = gannInternalValues[6, 0];
            else if (gannInternalValues[4, 6] != 0)
                this.ResistanceFive = gannInternalValues[6, 0];
            else if (gannInternalValues[6, 5] != 0)
                this.ResistanceFive = gannInternalValues[6, 0];
            else if (gannInternalValues[6, 2] != 0)
                this.ResistanceFive = gannInternalValues[6, 0];
        }

        private void CalculateSupport()
        {
            CalculateS1();
            CalculateS2();
            CalculateS3();
            CalculateS4();
            CalculateS5();
        }

        private void CalculateS1()
        {
            if (gannInternalValues[4, 1] != 0)
                this.SupportOne = gannInternalValues[4, 3];
            else if (gannInternalValues[2, 1] != 0)
                this.SupportOne = gannInternalValues[4, 2];
            else if (gannInternalValues[1, 2] != 0)
                this.SupportOne = gannInternalValues[3, 1];
            else if (gannInternalValues[1, 4] != 0)
                this.SupportOne = gannInternalValues[1, 1];
            else if (gannInternalValues[2, 5] != 0)
                this.SupportOne = gannInternalValues[1, 3];
            else if (gannInternalValues[4, 5] != 0)
                this.SupportOne = gannInternalValues[1, 5];
            else if (gannInternalValues[5, 4] != 0)
                this.SupportOne = gannInternalValues[3, 5];
            else if (gannInternalValues[5, 2] != 0)
                this.SupportOne = gannInternalValues[5, 5];
            else if (gannInternalValues[5, 0] != 0)
                this.SupportOne = gannInternalValues[5, 3];
            else if (gannInternalValues[2, 0] != 0)
                this.SupportOne = gannInternalValues[5, 1];
            else if (gannInternalValues[0, 1] != 0)
                this.SupportOne = gannInternalValues[3, 0];
            else if (gannInternalValues[0, 4] != 0)
                this.SupportOne = gannInternalValues[0, 0];
            else if (gannInternalValues[1, 6] != 0)
                this.SupportOne = gannInternalValues[0, 3];
            else if (gannInternalValues[4, 6] != 0)
                this.SupportOne = gannInternalValues[0, 6];
            else if (gannInternalValues[6, 5] != 0)
                this.SupportOne = gannInternalValues[3, 6];
            else if (gannInternalValues[6, 2] != 0)
                this.SupportOne = gannInternalValues[6, 6];
        }

        private void CalculateS2()
        {
            if (gannInternalValues[4, 1] != 0)
                this.SupportTwo = gannInternalValues[4, 4];
            else if (gannInternalValues[2, 1] != 0)
                this.SupportTwo = gannInternalValues[4, 3];
            else if (gannInternalValues[1, 2] != 0)
                this.SupportTwo = gannInternalValues[4, 2];
            else if (gannInternalValues[1, 4] != 0)
                this.SupportTwo = gannInternalValues[3, 1];
            else if (gannInternalValues[2, 5] != 0)
                this.SupportTwo = gannInternalValues[1, 1];
            else if (gannInternalValues[4, 5] != 0)
                this.SupportTwo = gannInternalValues[1, 3];
            else if (gannInternalValues[5, 4] != 0)
                this.SupportTwo = gannInternalValues[1, 5];
            else if (gannInternalValues[5, 2] != 0)
                this.SupportTwo = gannInternalValues[3, 5];
            else if (gannInternalValues[5, 0] != 0)
                this.SupportTwo = gannInternalValues[5, 5];
            else if (gannInternalValues[2, 0] != 0)
                this.SupportTwo = gannInternalValues[5, 3];
            else if (gannInternalValues[0, 1] != 0)
                this.SupportTwo = gannInternalValues[5, 1];
            else if (gannInternalValues[0, 4] != 0)
                this.SupportTwo = gannInternalValues[3, 0];
            else if (gannInternalValues[1, 6] != 0)
                this.SupportTwo = gannInternalValues[0, 0];
            else if (gannInternalValues[4, 6] != 0)
                this.SupportTwo = gannInternalValues[0, 3];
            else if (gannInternalValues[6, 5] != 0)
                this.SupportTwo = gannInternalValues[0, 6];
            else if (gannInternalValues[6, 2] != 0)
                this.SupportTwo = gannInternalValues[3, 6];
        }

        private void CalculateS3()
        {
            if (gannInternalValues[4, 1] != 0)
                this.SupportThree = gannInternalValues[3, 4];
            else if (gannInternalValues[2, 1] != 0)
                this.SupportThree = gannInternalValues[4, 4];
            else if (gannInternalValues[1, 2] != 0)
                this.SupportThree = gannInternalValues[4, 3];
            else if (gannInternalValues[1, 4] != 0)
                this.SupportThree = gannInternalValues[4, 2];
            else if (gannInternalValues[2, 5] != 0)
                this.SupportThree = gannInternalValues[3, 1];
            else if (gannInternalValues[4, 5] != 0)
                this.SupportThree = gannInternalValues[1, 1];
            else if (gannInternalValues[5, 4] != 0)
                this.SupportThree = gannInternalValues[1, 3];
            else if (gannInternalValues[5, 2] != 0)
                this.SupportThree = gannInternalValues[1, 5];
            else if (gannInternalValues[5, 0] != 0)
                this.SupportThree = gannInternalValues[3, 5];
            else if (gannInternalValues[2, 0] != 0)
                this.SupportThree = gannInternalValues[5, 5];
            else if (gannInternalValues[0, 1] != 0)
                this.SupportThree = gannInternalValues[5, 3];
            else if (gannInternalValues[0, 4] != 0)
                this.SupportThree = gannInternalValues[5, 1];
            else if (gannInternalValues[1, 6] != 0)
                this.SupportThree = gannInternalValues[3, 0];
            else if (gannInternalValues[4, 6] != 0)
                this.SupportThree = gannInternalValues[0, 0];
            else if (gannInternalValues[6, 5] != 0)
                this.SupportThree = gannInternalValues[0, 3];
            else if (gannInternalValues[6, 2] != 0)
                this.SupportThree = gannInternalValues[0, 6];
        }

        private void CalculateS4()
        {
            if (gannInternalValues[4, 1] != 0)
                this.SupportFour = gannInternalValues[2, 4];
            else if (gannInternalValues[2, 1] != 0)
                this.SupportFour = gannInternalValues[3, 4];
            else if (gannInternalValues[1, 2] != 0)
                this.SupportFour = gannInternalValues[4, 4];
            else if (gannInternalValues[1, 4] != 0)
                this.SupportFour = gannInternalValues[4, 3];
            else if (gannInternalValues[2, 5] != 0)
                this.SupportFour = gannInternalValues[4, 2];
            else if (gannInternalValues[4, 5] != 0)
                this.SupportFour = gannInternalValues[3, 1];
            else if (gannInternalValues[5, 4] != 0)
                this.SupportFour = gannInternalValues[1, 1];
            else if (gannInternalValues[5, 2] != 0)
                this.SupportFour = gannInternalValues[1, 3];
            else if (gannInternalValues[5, 0] != 0)
                this.SupportFour = gannInternalValues[1, 5];
            else if (gannInternalValues[2, 0] != 0)
                this.SupportFour = gannInternalValues[3, 5];
            else if (gannInternalValues[0, 1] != 0)
                this.SupportFour = gannInternalValues[5, 5];
            else if (gannInternalValues[0, 4] != 0)
                this.SupportFour = gannInternalValues[5, 3];
            else if (gannInternalValues[1, 6] != 0)
                this.SupportFour = gannInternalValues[5, 1];
            else if (gannInternalValues[4, 6] != 0)
                this.SupportFour = gannInternalValues[3, 0];
            else if (gannInternalValues[6, 5] != 0)
                this.SupportFour = gannInternalValues[0, 0];
            else if (gannInternalValues[6, 2] != 0)
                this.SupportFour = gannInternalValues[0, 3];
        }

        private void CalculateS5()
        {
            if (gannInternalValues[4, 1] != 0)
                this.SupportFive = gannInternalValues[2, 3];
            else if (gannInternalValues[2, 1] != 0)
                this.SupportFive = gannInternalValues[2, 4];
            else if (gannInternalValues[1, 2] != 0)
                this.SupportFive = gannInternalValues[3, 4];
            else if (gannInternalValues[1, 4] != 0)
                this.SupportFive = gannInternalValues[4, 4];
            else if (gannInternalValues[2, 5] != 0)
                this.SupportFive = gannInternalValues[4, 3];
            else if (gannInternalValues[4, 5] != 0)
                this.SupportFive = gannInternalValues[4, 2];
            else if (gannInternalValues[5, 4] != 0)
                this.SupportFive = gannInternalValues[3, 1];
            else if (gannInternalValues[5, 2] != 0)
                this.SupportFive = gannInternalValues[1, 1];
            else if (gannInternalValues[5, 0] != 0)
                this.SupportFive = gannInternalValues[1, 3];
            else if (gannInternalValues[2, 0] != 0)
                this.SupportFive = gannInternalValues[1, 5];
            else if (gannInternalValues[0, 1] != 0)
                this.SupportFive = gannInternalValues[3, 5];
            else if (gannInternalValues[0, 4] != 0)
                this.SupportFive = gannInternalValues[5, 5];
            else if (gannInternalValues[1, 6] != 0)
                this.SupportFive = gannInternalValues[5, 3];
            else if (gannInternalValues[4, 6] != 0)
                this.SupportFive = gannInternalValues[5, 1];
            else if (gannInternalValues[6, 5] != 0)
                this.SupportFive = gannInternalValues[3, 0];
            else if (gannInternalValues[6, 2] != 0)
                this.SupportFive = gannInternalValues[0, 0];
        }

        private void UpdateFirstDiagonal()
        {
            //first diagonal
            gannInternalValues[0, 0] = Math.Pow(firstValUp + Multiplier.Second, 2);
            gannInternalValues[1, 1] = Math.Pow(firstValDown + Multiplier.Second, 2);
            gannInternalValues[2, 2] = Math.Pow(secondValDown + Multiplier.Second, 2);
            gannInternalValues[3, 3] = Math.Pow(secondValDown, 2);
            gannInternalValues[4, 4] = Math.Pow(secondValDown + Multiplier.Sixth, 2);
            gannInternalValues[5, 5] = Math.Pow(firstValDown + Multiplier.Sixth, 2);
            gannInternalValues[6, 6] = Math.Pow(firstValUp + Multiplier.Sixth, 2);
        }

        private void UpdateSecondDiagonal()
        {
            //second diagonal
            gannInternalValues[0, 6] = Math.Pow(firstValUp + Multiplier.Forth, 2);
            gannInternalValues[1, 5] = Math.Pow(firstValDown + Multiplier.Forth, 2);
            gannInternalValues[2, 4] = Math.Pow(secondValDown + Multiplier.Forth, 2);
            gannInternalValues[4, 2] = Math.Pow(secondValDown + Multiplier.Eigth, 2);
            gannInternalValues[5, 1] = Math.Pow(firstValDown + Multiplier.Eigth, 2);
            gannInternalValues[6, 0] = Math.Pow(firstValUp + Multiplier.Eigth, 2);
        }

        private void UpdateMiddleVerticalRow()
        {
            //middle vertical row
            gannInternalValues[0, 3] = Math.Pow(firstValUp + Multiplier.Third, 2);
            gannInternalValues[1, 3] = Math.Pow(firstValDown + Multiplier.Third, 2);
            gannInternalValues[2, 3] = Math.Pow(secondValDown + Multiplier.Third, 2);
            gannInternalValues[4, 3] = Math.Pow(secondValDown + Multiplier.Seventh, 2);
            gannInternalValues[5, 3] = Math.Pow(firstValDown + Multiplier.Seventh, 2);
            gannInternalValues[6, 3] = Math.Pow(firstValUp + Multiplier.Seventh, 2);
        }

        private void UpdateMiddleHorizontalRow()
        {
            //middle horizontal row
            gannInternalValues[3, 0] = Math.Pow(firstValUp + Multiplier.First, 2);
            gannInternalValues[3, 1] = Math.Pow(firstValDown + Multiplier.First, 2);
            gannInternalValues[3, 2] = Math.Pow(secondValDown + Multiplier.First, 2);
            gannInternalValues[3, 4] = Math.Pow(secondValDown + Multiplier.Fifth, 2);
            gannInternalValues[3, 5] = Math.Pow(firstValDown + Multiplier.Fifth, 2);
            gannInternalValues[3, 6] = Math.Pow(firstValUp + Multiplier.Fifth, 2);
        }

        private void UpdateCalculatedValues()
        {
            //conditional cells
            gannInternalValues[0, 1] = (cmp >= gannInternalValues[0, 0] && cmp < gannInternalValues[0, 3]) ? cmp : 0;
            gannInternalValues[0, 4] = (cmp >= gannInternalValues[0, 3] && cmp < gannInternalValues[0, 6]) ? cmp : 0;

            gannInternalValues[1, 2] = (cmp >= gannInternalValues[1, 1] && cmp < gannInternalValues[1, 3]) ? cmp : 0;
            gannInternalValues[1, 4] = (cmp >= gannInternalValues[1, 3] && cmp < gannInternalValues[1, 5]) ? cmp : 0;
            gannInternalValues[1, 6] = (cmp >= gannInternalValues[0, 6] && cmp < gannInternalValues[3, 6]) ? cmp : 0;

            gannInternalValues[2, 0] = (cmp >= gannInternalValues[3, 0] && cmp < gannInternalValues[0, 0]) ? cmp : 0;
            gannInternalValues[2, 1] = (cmp >= gannInternalValues[3, 1] && cmp < gannInternalValues[1, 1]) ? cmp : 0;
            gannInternalValues[2, 5] = (cmp >= gannInternalValues[1, 5] && cmp < gannInternalValues[3, 5]) ? cmp : 0;

            gannInternalValues[4, 1] = (cmp >= gannInternalValues[4, 2] && cmp < gannInternalValues[3, 1]) ? cmp : 0;
            gannInternalValues[4, 5] = (cmp >= gannInternalValues[3, 5] && cmp < gannInternalValues[5, 5]) ? cmp : 0;
            gannInternalValues[4, 6] = (cmp >= gannInternalValues[3, 6] && cmp < gannInternalValues[6, 6]) ? cmp : 0;

            gannInternalValues[5, 0] = (cmp >= gannInternalValues[5, 1] && cmp < gannInternalValues[3, 0]) ? cmp : 0;
            gannInternalValues[5, 2] = (cmp >= gannInternalValues[5, 3] && cmp < gannInternalValues[5, 1]) ? cmp : 0;
            gannInternalValues[5, 4] = (cmp >= gannInternalValues[5, 5] && cmp < gannInternalValues[5, 3]) ? cmp : 0;

            gannInternalValues[6, 2] = (cmp >= gannInternalValues[6, 3] && cmp < gannInternalValues[6, 0]) ? cmp : 0;
            gannInternalValues[6, 5] = (cmp >= gannInternalValues[6, 6] && cmp < gannInternalValues[6, 3]) ? cmp : 0;
        }
    }

    
}
