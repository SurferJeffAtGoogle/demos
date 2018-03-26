﻿// Copyright (c) 2018 Google LLC.
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not
// use this file except in compliance with the License. You may obtain a copy of
// the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
// WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
// License for the specific language governing permissions and limitations under
// the License.

using System;
using Google.Cloud.Vision.V1;
using Newtonsoft.Json;

namespace image
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = ImageAnnotatorClient.Create();
            var results = client.DetectSafeSearch(
                Image.FromFile(@"C:\users\rennie\Pictures\sorethroat.jpg"));
            JsonDumper.Dump(results);
        }
    }
}