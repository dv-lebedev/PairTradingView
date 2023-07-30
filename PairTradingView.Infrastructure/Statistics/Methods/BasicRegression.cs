﻿/*
Copyright(c) 2023 Denis Lebedev

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System;
using System.Linq;

namespace PairTradingView.Shared.Statistics.Methods
{
    public class BasicRegression : IRegressionMethod
    {
        private double b0;
        private double b1;
        private double rValue;
        private double rSquared;

        public double[] Coefs
        {
            get
            {
                return new double[] { b0, b1 };
            }
        }

        public double[] RSquaredValues
        {
            get
            {
                return new double[] { rSquared };
            }
        }

        public double[] RValues
        {
            get
            {
                return new double[] { rValue };
            }
        }

        public void Compute(double[] y, params double[][] xn)
        {
            if (y == null) throw new ArgumentNullException("y");
            if (xn == null) throw new ArgumentNullException("xn");
            if (xn.Length > 1) throw new ArgumentException("Number of arrays is more than 1.");
            if (y.Length != xn[0].Length) throw new DifferentLengthException();

            double[] x = xn[0];

            int N = x.Length;
            double xAverage = x.Average();
            double yAverage = y.Average();
            double sx2 = MathUtils.Pow(x, 2) / N - Math.Pow(xAverage, 2);
            double xy = MathUtils.MultiplyArrays(x, y);
            double covariation = xy / N - xAverage * yAverage;
            b1 = covariation / sx2;
            b0 = yAverage - b1 * xAverage;
            rValue = b1 * (MathUtils.GetStandardDeviation(x) / MathUtils.GetStandardDeviation(y));
            rSquared = Math.Pow(rValue, 2);
        }
    }
}
