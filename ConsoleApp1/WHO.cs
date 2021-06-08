using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace ConsoleApp1
{
    

    // A generic parameter class with attributes for all possible action
    // parameters of any action type.
    [DataContract]
    class GenericActionParameters
    {
        [DataMember(EmitDefaultValue = false)]
        public List<string> location;
        [DataMember(EmitDefaultValue = false)]
        public int testQuality;
        [DataMember(EmitDefaultValue = false)]
        public int quarantinePeriod;
        [DataMember(EmitDefaultValue = false)]
        public int quantity;
        [DataMember(EmitDefaultValue = false)]
        public bool symptomaticOnly;
        [DataMember(EmitDefaultValue = false)]
        public bool vulnerablePeople;
        [DataMember(EmitDefaultValue = false)]
        public int ageThreshold;
        [DataMember(EmitDefaultValue = false)]
        public int distance;
        [DataMember(EmitDefaultValue = false)]
        public int amountInvested;
        [DataMember(EmitDefaultValue = false)]
        public int amountLoaned;
        [DataMember(EmitDefaultValue = false)]
        public int[] maskProvisionLevel;
    }

    // A generic action class holding generic parameters
    [DataContract]
    class GenericAction
    {
        [DataMember]
        public uint id;
        [DataMember]
        public string action;
        [DataMember]
        public string mode;
        [DataMember]
        public GenericActionParameters parameters;
    }



    [DataContract]
    class TestResultRequest
    {
        [DataMember]
        public List<List<string>> locations;
    }


    [DataContract]
    class TestResultElement
    {
        [DataMember]
        public int total;
        [DataMember]
        public int positive;
        [DataMember]
        public List<string> location;
    }

    [DataContract]
    class GetSimStatus
    {
        [DataMember]
        public bool isWhoTurn;
        [DataMember]
        public int turnCount;
        [DataMember]
        public int budget;
        [DataMember]
        public bool vacReady;
    }

    [DataContract]
    class UpdateSimStatus
    {
        [DataMember]
        public bool isWhoTurn;
    }

    [DataContract]
    class SearchTotalRequest
    {
        [DataMember]
        public List<List<string>> locations;
    }

    [DataContract]
    class SearchTotalElement
    {
        [DataMember]
        public List<string> location;
        [DataMember]
        public int uninfected;
        [DataMember]
        public int asymptomaticInfectedNotInfectious;
        [DataMember]
        public int asymptomaticInfectedInfectious;
        [DataMember]
        public int symptomatic;
        [DataMember]
        public int seriousInfection;
        [DataMember]
        public int dead;
        [DataMember]
        public int recoveredImmune;


    }



    class WHO
    {
        int budget = Constant.INITIAL_BUDGET;
        private uint staticID = 20000;
        List<uint> actionIds = new List<uint>();
        List<GenericAction> actions = new List<GenericAction>();
        List<List<string>> locations = new List<List<string>>();
        int[] maskType = new int[] { 1 };
        Dictionary<string, uint> locationId = new Dictionary<string, uint>();       
        public WHO(List<List<string>> locations)
        {
            uint id = 100;
            this.locations = locations;
            foreach(List<string> location in locations)
            {
                locationId.Add(String.Join("", location), id);
                id += 100;
            }
        }

        public void TestAndIsolation(uint id, List<string> location, int testQuality, int quarantinePeriod, int quantity, bool symptomaticOnly)
        {
            if (!actionIds.Contains(id) && this.budget >= Constant.TEST_ISOLATION_COST)
            {
                var testAction = new GenericAction();
                testAction.id = id;
                testAction.mode = "create";
                testAction.action = "testAndIsolation";

                testAction.parameters = new GenericActionParameters();
                testAction.parameters.testQuality = testQuality;
                testAction.parameters.quarantinePeriod = quarantinePeriod;
                testAction.parameters.quantity = quantity;
                testAction.parameters.location = location;
                testAction.parameters.symptomaticOnly = symptomaticOnly;
                actionIds.Add(id);
                actions.Add(testAction);

                this.budget -= Constant.TEST_ISOLATION_COST;
            }
            if(this.budget < Constant.TEST_ISOLATION_COST)
            {
                Delete(id);
            }
        }

        public void StayAtHome(uint id, List<string> location)
        {
            if (!actionIds.Contains(id + 1) && this.budget >= Constant.STAY_AT_HOME_COST)
            {
                var StayAtHome = new GenericAction();
                StayAtHome.id = id + 1;
                StayAtHome.mode = "create";
                StayAtHome.action = "stayAtHome";

                StayAtHome.parameters = new GenericActionParameters();
                StayAtHome.parameters.location = location;
                actionIds.Add(id + 1);
                actions.Add(StayAtHome);

                this.budget -= Constant.STAY_AT_HOME_COST;
            }
            if(this.budget < Constant.STAY_AT_HOME_COST)
            {
                Delete(id + 1);
            }
        }

        public void CloseSchool(uint id, List<string> location)
        {
            if (!actionIds.Contains(id + 2) && this.budget >= Constant.CLOSE_SCHOOL_COST)
            {
                var closeSchool = new GenericAction();
                closeSchool.id = id + 2;
                closeSchool.mode = "create";
                closeSchool.action = "closeSchools";

                closeSchool.parameters = new GenericActionParameters();
                closeSchool.parameters.location = location;
                actionIds.Add(id + 2);
                actions.Add(closeSchool);

                this.budget -= Constant.CLOSE_SCHOOL_COST;
            }
            if (this.budget < Constant.CLOSE_SCHOOL_COST)
            {
                Delete(id + 2);
            }
        }

        public void CloseRecreational(uint id, List<string> location)
        {
            if (!actionIds.Contains(id + 3) && this.budget >= Constant.CLOSE_RECREATIONAL_COST)
            {
                var closeRecreational = new GenericAction();
                closeRecreational.id = id + 3;
                closeRecreational.mode = "create";
                closeRecreational.action = "closeRecreationalLocations";

                closeRecreational.parameters = new GenericActionParameters();
                closeRecreational.parameters.location = location;
                actionIds.Add(id + 3);
                actions.Add(closeRecreational);

                this.budget -= Constant.CLOSE_RECREATIONAL_COST;
            }
            if (this.budget < Constant.CLOSE_RECREATIONAL_COST)
            {
                Delete(id + 3);
            }
        }

        public void Shielding(uint id, List<string> location, bool vulnerablePeople, int ageThreshold)
        {
            if (!actionIds.Contains(id + 4) && this.budget >= Constant.SHIELDING_COST)
            {
                var shielding = new GenericAction();
                shielding.id = id + 4;
                shielding.mode = "create";
                shielding.action = "shieldingProgram";

                shielding.parameters = new GenericActionParameters();
                shielding.parameters.location = location;
                shielding.parameters.vulnerablePeople = vulnerablePeople;
                shielding.parameters.ageThreshold = ageThreshold;
                actionIds.Add(id + 4);
                actions.Add(shielding);

                this.budget -= Constant.SHIELDING_COST;
            }
            if (this.budget < Constant.SHIELDING_COST)
            {
                Delete(id + 4);
            }
        }

        public void MovementRestriction(uint id, List<string> location, int distance)
        {
            if (!actionIds.Contains(id + 5) && this.budget >= Constant.MOVEMENT_RESTRICTION_COST)
            {
                var movementRistriction = new GenericAction();
                movementRistriction.id = id + 5;
                movementRistriction.mode = "create";
                movementRistriction.action = "movementRestrictions";

                movementRistriction.parameters = new GenericActionParameters();
                movementRistriction.parameters.location = location;
                movementRistriction.parameters.distance = distance;
                actionIds.Add(id + 5);
                actions.Add(movementRistriction);

                this.budget -= Constant.MOVEMENT_RESTRICTION_COST;
            }
            if (this.budget < Constant.MOVEMENT_RESTRICTION_COST)
            {
                Delete(id + 5);
            }
        }

        public void CloseBoder(uint id, List<string> location)
        {
            if (!actionIds.Contains(id + 6) && this.budget >= Constant.CLOSE_BODER_COST)
            {
                var closeBoder = new GenericAction();
                closeBoder.id = id + 6;
                closeBoder.mode = "create";
                closeBoder.action = "closeBorders";

                closeBoder.parameters = new GenericActionParameters();
                closeBoder.parameters.location = location;
                actionIds.Add(id + 6);
                actions.Add(closeBoder);

                this.budget -= Constant.CLOSE_BODER_COST;
            }
            if (this.budget < Constant.CLOSE_BODER_COST)
            {
                Delete(id + 6);
            }
        }

        public void InvestVac(uint id, int investAmount)
        {
            if (!actionIds.Contains(id + 7))
            {
                var investVac = new GenericAction();
                investVac.id = id + 7;
                investVac.mode = "create";
                investVac.action = "investInVaccine";

                investVac.parameters = new GenericActionParameters();
                investVac.parameters.amountInvested = investAmount;
                actionIds.Add(id + 7);
                actions.Add(investVac);
            }
        }

        public void Furlough(uint id,List<string> location, int amountInvested)
        {
            if (!actionIds.Contains(id + 8))
            {


                var furlough = new GenericAction();
                furlough.id = id + 8;
                furlough.mode = "create";
                furlough.action = "furlough";

                furlough.parameters = new GenericActionParameters();
                furlough.parameters.amountInvested = amountInvested;
                furlough.parameters.location = location;
                actionIds.Add(id + 8);
                actions.Add(furlough);
            }
        }

        public void InformationPress(uint id, List<string> location, int amountInvested)
        {
            if (!actionIds.Contains(id + 9) && this.budget >= Constant.INFO_PRESS_INVEST_HIGH)
            {
                var infoPress = new GenericAction();
                infoPress.id = id + 9;
                infoPress.mode = "create";
                infoPress.action = "infoPressRelease";

                infoPress.parameters = new GenericActionParameters();
                infoPress.parameters.amountInvested = amountInvested;
                infoPress.parameters.location = location;
                actionIds.Add(id + 9);
                actions.Add(infoPress);
                this.budget -= Constant.INFO_PRESS_INVEST_HIGH;
            }
            if (this.budget < Constant.INFO_PRESS_INVEST_HIGH)
            {
                Delete(id + 9);
            }
        }

        public void Loan(uint id, int amountLoan)
        {
            if (!actionIds.Contains(id + 10))
            {
                var loan = new GenericAction();
                loan.id = id + 10;
                loan.mode = "create";
                loan.action = "loan";

                loan.parameters = new GenericActionParameters();
                loan.parameters.amountLoaned = amountLoan;
                actionIds.Add(id + 10);
                actions.Add(loan);
            }
        }

        public void MaskMandate(uint id, List<string> location, int[] maskProvisionLevel)
        {
            if (!actionIds.Contains(id + 11) && this.budget >= Constant.MASK_MANDATE_COST)
            {
                var maskMandate = new GenericAction();
                maskMandate.id = id + 11;
                maskMandate.mode = "create";
                maskMandate.action = "maskMandate";

                maskMandate.parameters = new GenericActionParameters();
                maskMandate.parameters.location = location;
                maskMandate.parameters.maskProvisionLevel = maskProvisionLevel;
                actionIds.Add(id + 11);
                actions.Add(maskMandate);

                this.budget -= Constant.MASK_MANDATE_COST;
            }
            if (this.budget < Constant.MASK_MANDATE_COST)
            {
                Delete(id + 11);
            }
        }

        public void HealthDrive(uint id, List<string> location)
        {
            if (!actionIds.Contains(id + 12) && this.budget >= Constant.HEALTH_DRIVE_COST)
            {
                var healthDrive = new GenericAction();
                healthDrive.id = id + 12;
                healthDrive.mode = "create";
                healthDrive.action = "healthDrive";

                healthDrive.parameters = new GenericActionParameters();
                healthDrive.parameters.location = location;
                actionIds.Add(id + 12);
                actions.Add(healthDrive);
                this.budget -= Constant.HEALTH_DRIVE_COST;
            }
            if (this.budget < Constant.HEALTH_DRIVE_COST)
            {
                Delete(id + 12);
            }
        }

        public void InvestHealthService(uint id, int amountInvested)
        {
            if (!actionIds.Contains(id + 13))
            {
                var investHealthService = new GenericAction();
                investHealthService.id = id + 13;
                investHealthService.mode = "create";
                investHealthService.action = "investInHealthServices";

                investHealthService.parameters = new GenericActionParameters();
                investHealthService.parameters.amountInvested = amountInvested;
                actionIds.Add(id + 13);
                actions.Add(investHealthService);
            }
        }

        public void SocialDistancing(uint id, List<string> location, int distance)
        {
            if (!actionIds.Contains(id + 14) && this.budget >= Constant.SOCIAL_DISTANCEING_COST)
            {
                var socialDistance = new GenericAction();
                socialDistance.id = id + 14;
                socialDistance.mode = "create";
                socialDistance.action = "socialDistancingMandate";

                socialDistance.parameters = new GenericActionParameters();
                socialDistance.parameters.location = location;
                socialDistance.parameters.distance = distance;
                actionIds.Add(id + 14);
                actions.Add(socialDistance);

                this.budget -= Constant.SOCIAL_DISTANCEING_COST;
            }
            if (this.budget < Constant.SOCIAL_DISTANCEING_COST)
            {
                Delete(id + 14);
            }
        }

        public void Curfew(uint id, List<string> location)
        {
            if (!actionIds.Contains(id + 15) && this.budget >= Constant.CURFEW_COST)
            {
                var socialDistance = new GenericAction();
                socialDistance.id = id + 15;
                socialDistance.mode = "create";
                socialDistance.action = "curfew";

                socialDistance.parameters = new GenericActionParameters();
                socialDistance.parameters.location = location;
                actionIds.Add(id + 15);
                actions.Add(socialDistance);
                this.budget -= Constant.CURFEW_COST;
            }
            if (this.budget < Constant.CURFEW_COST)
            {
                Delete(id + 15);
            }
        }

        public void AdministerVaccine(uint id, List<string> location)
        {
            if(!actionIds.Contains(id+16) && this.budget >= Constant.VACCINE_MAND_COST)
            {
                var vaccMandate = new GenericAction();
                vaccMandate.id = id + 16;
                vaccMandate.mode = "create";
                vaccMandate.action = "administerVaccine";

                vaccMandate.parameters = new GenericActionParameters();
                vaccMandate.parameters.location = location;
                actionIds.Add(id + 16);
                actions.Add(vaccMandate);

                this.budget -= Constant.VACCINE_MAND_COST;
            }
            if(this.budget < Constant.VACCINE_MAND_COST)
            {
                Delete(id + 16);
            }
        }

        public void Delete(uint id)
        {
            if (actionIds.Contains(id))
            {
                var delete = new GenericAction();
                delete.id = id;
                delete.mode = "delete";
                actions.Add(delete);
                actionIds.Remove(id);
            }
        }

        public TestResultRequest searchTestResult(List<List<string>> location)
        {
            var testResult = new TestResultRequest();
            testResult.locations = location;

            return testResult;
        }

        public void UpdateSimStatus()
        {
            var update = new UpdateSimStatus();
            update.isWhoTurn = false;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://127.0.0.1:8080/status");
            request.Method = "POST";

            // Convert the actions list to JSON and add it to the request
            var serializer = new DataContractJsonSerializer(typeof(UpdateSimStatus));
            serializer.WriteObject(request.GetRequestStream(), update);
            request.GetResponse().Close();

        }

        public void SendActionRequest()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://127.0.0.1:8080/actions");
            request.Method = "POST";

            // Convert the actions list to JSON and add it to the request
            var serializer = new DataContractJsonSerializer(typeof(List<GenericAction>));
            serializer.WriteObject(request.GetRequestStream(), actions);

            // Send the request and await a response
            request.GetResponse().Close();
            actions.Clear();
        }

        public List<TestResultElement> SendTestResultRequest(TestResultRequest testResult)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://127.0.0.1:8080/info/test-results");
            request.Method = "POST";

            
            // Convert the actions list to JSON and add it to the request
            var serializer = new DataContractJsonSerializer(typeof(TestResultRequest));
            serializer.WriteObject(request.GetRequestStream(), testResult);
            WebResponse respond = request.GetResponse();
            var respondStream = respond.GetResponseStream();
            var parser = new DataContractJsonSerializer(typeof(List<TestResultElement>));
            List<TestResultElement> results = parser.ReadObject(respondStream) as List<TestResultElement>;
            respond.Close();

            return results;
            // what I get from GetRespond()

        }

        public GetSimStatus GetSimStatusRequest()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://127.0.0.1:8080/status");
            request.Method = "GET";
            WebResponse respond = request.GetResponse();
            var respondStream = respond.GetResponseStream();
            var parser = new DataContractJsonSerializer(typeof(GetSimStatus));
            GetSimStatus simStatus = parser.ReadObject(respondStream) as GetSimStatus;
            respond.Close();

            return simStatus;
        }

        public SearchTotalRequest SearchTotalRequest(List<List<string>> location)
        {
            var searchTotal = new SearchTotalRequest();
            searchTotal.locations = location;

            return searchTotal;
        }

        public void SendSearchTotalRequest(SearchTotalRequest searchTotal)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://127.0.0.1:8080/info/totals");
            request.Method = "POST";

            var serializer = new DataContractJsonSerializer(typeof(SearchTotalRequest));
            serializer.WriteObject(request.GetRequestStream(), searchTotal);
            WebResponse respond = request.GetResponse();
            var respondStream = respond.GetResponseStream();
            var parser = new DataContractJsonSerializer(typeof(List<SearchTotalElement>));
            List<SearchTotalElement> totalElements = parser.ReadObject(respondStream) as List<SearchTotalElement>;
            respond.Close();

            foreach(SearchTotalElement element in totalElements)
            {
                Console.WriteLine("Got " + element.dead + " Death");
            }

        }

        

        public void WHOActionLogic()
        {
            // get SimStatus
            GetSimStatus simStatus = GetSimStatusRequest();
            this.budget += simStatus.budget;
            if (simStatus.isWhoTurn)
            {
                // testing
                //uint id = 0;
                foreach (List<string> location in this.locations)
                {
                    uint id = locationId[String.Join("", location)];
                    TestAndIsolation(id, location, 0, 21, 100, false);
                    if (simStatus.vacReady)
                    {
                        AdministerVaccine(id, location);
                    }
                }
                if (actions.Count > 0)
                {
                    SendActionRequest();
                    actions.Clear();
                }

                
                

                // invest in vaccine
                if(this.budget > Constant.VACCINE_INVEST_HIGH)
                {
                    InvestVac(staticID, Constant.VACCINE_INVEST_HIGH);
                    
                }
                else if (this.budget > Constant.VACCINE_INVEST_LOW)
                {
                    InvestVac(staticID, Constant.VACCINE_INVEST_LOW);
                    
                }
                if (actions.Count > 0)
                {
                    SendActionRequest();
                    actions.Clear();
                }

                // take loan
                if (this.budget < Constant.BUDGET_THRESHOLD_2 && this.budget > Constant.BUDGET_THRESHOLD_1)
                {
                    Loan(staticID, Constant.LOAN_GAIN_LOW);
                    
                }
                else if (this.budget <= Constant.BUDGET_THRESHOLD_1)
                {
                    Loan(staticID, Constant.LOAN_GAIN_HIGH);

                }
                else
                {
                    Delete(staticID + 10);
                }

                if (actions.Count > 0)
                {
                    SendActionRequest();
                    actions.Clear();
                }


                // get test result
                TestResultRequest resultRequest = searchTestResult(this.locations);
                List<TestResultElement> results = SendTestResultRequest(resultRequest);

                // lockDown logic
                if (results.Count > 0)
                {

                    foreach (TestResultElement element in results)
                    {
                        double infectedRate = (double)element.positive / (double)element.total;
                        uint id = locationId[String.Join("", element.location)];
                        if (0 <= infectedRate && infectedRate <= Constant.INFECTED_RATE_THRESHOLD_1)
                        {
                            
                            Shielding(id, element.location, true, 60);
                            
                            SocialDistancing(id, element.location, Constant.RESTRICT_DISTANCE);
                            
                            MovementRestriction(id, element.location, Constant.RESTRICT_DISTANCE);

                            InformationPress(id, element.location, Constant.INFO_PRESS_INVEST_HIGH);

                            //delete stay at home, close school, close recreational
                            Delete(id + 1);
                            Delete(id + 2);
                            Delete(id + 3);

                            //delete health drive
                            Delete(id + 12);
                            //delete close boder
                            Delete(id + 6);
                            //delete curfew
                            Delete(id + 15);
                            //delete mask
                            Delete(id + 11);
                        }
                        else if (Constant.INFECTED_RATE_THRESHOLD_1 <= infectedRate && infectedRate <= Constant.INFECTED_RATE_THRESHOLD_2)
                        {
                            
                            Shielding(id, element.location, true, 60);
                            
                            SocialDistancing(id, element.location, Constant.RESTRICT_DISTANCE);
                            
                            MovementRestriction(id, element.location, Constant.RESTRICT_DISTANCE);
                            
                            StayAtHome(id, element.location);
                            
                            CloseSchool(id, element.location);
                            
                            CloseRecreational(id, element.location);
                            
                            InformationPress(id, element.location, Constant.INFO_PRESS_INVEST_HIGH);

                            HealthDrive(id, element.location);

                            MaskMandate(id, element.location,maskType);
                            //delete close boder
                            Delete(id + 6);
                            //delete curfew
                            Delete(id + 15);
                        }
                        else if(infectedRate >= Constant.INFECTED_RATE_THRESHOLD_2)
                        {
                            Shielding(id, element.location, true, 60);
                            
                            SocialDistancing(id, element.location, Constant.RESTRICT_DISTANCE);
                            
                            MovementRestriction(id, element.location, Constant.RESTRICT_DISTANCE);
                            
                            StayAtHome(id, element.location);
                            
                            CloseSchool(id, element.location);
                            
                            CloseRecreational(id, element.location);
                            
                            InformationPress(id, element.location, Constant.INFO_PRESS_INVEST_HIGH);
                            
                            CloseBoder(id, element.location);
                            
                            Curfew(id, element.location);

                            HealthDrive(id, element.location);

                            MaskMandate(id, element.location, maskType);
                        }
                    }
                }

                if (actions.Count > 0)
                {
                    SendActionRequest();
                    actions.Clear();
                }

                //end WHO turn
                UpdateSimStatus();
            }




        }

        public void testTotal()
        {
            //List<List<string>> locations = new List<List<string>>();
            //List<string> location = new List<string>();
            //location.Add("A0");
            //locations.Add(location);

            SearchTotalRequest searchTotalRequest = SearchTotalRequest(this.locations);
            SendSearchTotalRequest(searchTotalRequest);
        }
    }
}
