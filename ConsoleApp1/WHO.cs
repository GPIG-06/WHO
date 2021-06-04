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
        public List<List<string>> location;
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
        public List<List<string>> location;
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
        int budget = 0;
        List<uint> actionIds = new List<uint>();
        
        List<GenericAction> actions = new List<GenericAction>();
        List<List<string>> locations = new List<List<string>>();
        public WHO(List<List<string>> locations)
        {
            this.locations = locations;
        }

        public void TestAndIsolation(uint id, List<string> location, int testQuality, int quarantinePeriod, int quantity, bool symptomaticOnly)
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

            actions.Add(testAction);
        }

        public void StayAtHome(uint id, List<string> location)
        {
            var StayAtHome = new GenericAction();
            StayAtHome.id = id;
            StayAtHome.mode = "create";
            StayAtHome.action = "stayAtHome";

            StayAtHome.parameters = new GenericActionParameters();
            StayAtHome.parameters.location = location;

            actions.Add(StayAtHome);
        }

        public void CloseSchool(uint id, List<string> location)
        {
            var closeSchool = new GenericAction();
            closeSchool.id = id;
            closeSchool.mode = "create";
            closeSchool.action = "closeSchools";

            closeSchool.parameters = new GenericActionParameters();
            closeSchool.parameters.location = location;

            actions.Add(closeSchool);
        }

        public void CloseRecreational(uint id, List<string> location)
        {
            var closeRecreational = new GenericAction();
            closeRecreational.id = id;
            closeRecreational.mode = "create";
            closeRecreational.action = "closeSchools";

            closeRecreational.parameters = new GenericActionParameters();
            closeRecreational.parameters.location = location;

            actions.Add(closeRecreational);
        }

        public void Shielding(uint id, List<string> location, bool vulnerablePeople, int ageThreshold)
        {
            var shielding = new GenericAction();
            shielding.id = id;
            shielding.mode = "create";
            shielding.action = "shieldingProgram";

            shielding.parameters = new GenericActionParameters();
            shielding.parameters.location = location;
            shielding.parameters.vulnerablePeople = vulnerablePeople;
            shielding.parameters.ageThreshold = ageThreshold;

            actions.Add(shielding);
        }

        public void MovementRestriction(uint id, List<string> location, int distance)
        {
            var movementRistriction = new GenericAction();
            movementRistriction.id = id;
            movementRistriction.mode = "create";
            movementRistriction.action = "movementRestrictions";

            movementRistriction.parameters = new GenericActionParameters();
            movementRistriction.parameters.location = location;
            movementRistriction.parameters.distance = distance;

            actions.Add(movementRistriction);
        }

        public void CloseBoder(uint id, List<string> location)
        {
            var closeBoder = new GenericAction();
            closeBoder.id = id;
            closeBoder.mode = "create";
            closeBoder.action = "closeBorders";

            closeBoder.parameters = new GenericActionParameters();
            closeBoder.parameters.location = location;

            actions.Add(closeBoder);
        }

        public void InvestVac(uint id, int investAmount)
        {
            var investVac = new GenericAction();
            investVac.id = id;
            investVac.mode = "create";
            investVac.action = "investInVaccine";

            investVac.parameters = new GenericActionParameters();
            investVac.parameters.amountInvested = investAmount;

            actions.Add(investVac);
        }

        public void Furlough(uint id,List<string> location, int amountInvested)
        {
            var furlough = new GenericAction();
            furlough.id = id;
            furlough.mode = "create";
            furlough.action = "furlough";

            furlough.parameters = new GenericActionParameters();
            furlough.parameters.amountInvested = amountInvested;
            furlough.parameters.location = location;

            actions.Add(furlough);
        }

        public void InformationPress(uint id, List<string> location, int amountInvested)
        {
            var infoPress = new GenericAction();
            infoPress.id = id;
            infoPress.mode = "create";
            infoPress.action = "infoPressRelease";

            infoPress.parameters = new GenericActionParameters();
            infoPress.parameters.amountInvested = amountInvested;
            infoPress.parameters.location = location;

            actions.Add(infoPress);
        }

        public void Loan(uint id, int amountLoan)
        {
            var loan = new GenericAction();
            loan.id = id;
            loan.mode = "create";
            loan.action = "loan";

            loan.parameters = new GenericActionParameters();
            loan.parameters.amountLoaned = amountLoan;

            actions.Add(loan);
        }

        public void MaskMandate(uint id, List<string> location, int[] maskProvisionLevel)
        {
            var maskMandate = new GenericAction();
            maskMandate.id = id;
            maskMandate.mode = "create";
            maskMandate.action = "maskMandate";

            maskMandate.parameters = new GenericActionParameters();
            maskMandate.parameters.location = location;
            maskMandate.parameters.maskProvisionLevel = maskProvisionLevel;

            actions.Add(maskMandate);
        }

        public void HealthDrive(uint id, List<string> location)
        {
            var healthDrive = new GenericAction();
            healthDrive.id = id;
            healthDrive.mode = "create";
            healthDrive.action = "healthDrive";

            healthDrive.parameters = new GenericActionParameters();
            healthDrive.parameters.location = location;

            actions.Add(healthDrive);
        }

        public void InvestHealthService(uint id, int amountInvested)
        {
            var investHealthService = new GenericAction();
            investHealthService.id = id;
            investHealthService.mode = "create";
            investHealthService.action = "investInHealthServices ";

            investHealthService.parameters = new GenericActionParameters();
            investHealthService.parameters.amountInvested = amountInvested;

            actions.Add(investHealthService);
        }

        public void SocialDistancing(uint id, List<string> location, int distance)
        {
            var socialDistance = new GenericAction();
            socialDistance.id = id;
            socialDistance.mode = "create";
            socialDistance.action = "socialDistancingMandate ";

            socialDistance.parameters = new GenericActionParameters();
            socialDistance.parameters.location = location;
            socialDistance.parameters.distance = distance;

            actions.Add(socialDistance);
        }

        public void Curfew(uint id, List<string> location)
        {
            var socialDistance = new GenericAction();
            socialDistance.id = id;
            socialDistance.mode = "create";
            socialDistance.action = "curfew ";

            socialDistance.parameters = new GenericActionParameters();
            socialDistance.parameters.location = location;

            actions.Add(socialDistance);
        }

        public void Delete(uint id)
        {
            var delete = new GenericAction();
            delete.id = id;
            delete.mode = "delete";
        }

        public TestResultRequest searchTestResult(List<List<string>> location)
        {
            var testResult = new TestResultRequest();
            testResult.location = location;

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
            request.GetResponse();

        }

        public void SendActionRequest()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://127.0.0.1:8080/actions");
            request.Method = "POST";

            // Convert the actions list to JSON and add it to the request
            var serializer = new DataContractJsonSerializer(typeof(List<GenericAction>));
            serializer.WriteObject(request.GetRequestStream(), actions);

            // Send the request and await a response
            request.GetResponse();
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

            return simStatus;
        }

        public SearchTotalRequest SearchTotalRequest(List<List<string>> location)
        {
            var searchTotal = new SearchTotalRequest();
            searchTotal.location = location;

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

            foreach(SearchTotalElement element in totalElements)
            {
                Console.WriteLine("Got " + element.dead + " Death");
            }

        }

        

        public void WHOActionLogic()
        {
            GetSimStatus simStatus = GetSimStatusRequest();
            this.budget = simStatus.budget;
            if (simStatus.isWhoTurn)
            {

                // testing
                uint id = 0;
                foreach (List<string> location in this.locations)
                {
                    TestAndIsolation(id, location, 0, 21, 100, false);
                    this.actionIds.Add(id);
                    id++;
                }
                SendActionRequest();
                actions.Clear();

                if(this.budget > Constant.VACCINE_INVEST_HIGH)
                {
                    InvestVac(id, Constant.VACCINE_INVEST_HIGH);
                    id++;
                    this.actionIds.Add(id);
                }
                else if (this.budget > Constant.VACCINE_INVEST_LOW)
                {
                    InvestVac(id, Constant.VACCINE_INVEST_LOW);
                    id++;
                    this.actionIds.Add(id);
                }
                SendActionRequest();
                actions.Clear();



                TestResultRequest resultRequest = searchTestResult(this.locations);
                List<TestResultElement> results = SendTestResultRequest(resultRequest);

                if (results.Count > 0)
                {

                    foreach (TestResultElement element in results)
                    {
                        double infectedRate = (double)element.positive / (double)element.total;
                        if (0 <= infectedRate && infectedRate <= Constant.INFECTED_RATE_THRESHOLD_1)
                        {
                            Shielding(id, element.location, true, 60);
                            this.actionIds.Add(id);
                            id++;
                            SocialDistancing(id, element.location, Constant.RESTRICT_DISTANCE);
                            this.actionIds.Add(id);
                            id++;
                            MovementRestriction(id, element.location, Constant.RESTRICT_DISTANCE);
                            this.actionIds.Add(id);
                            id++;

                        }
                        else if (Constant.INFECTED_RATE_THRESHOLD_1 <= infectedRate && infectedRate <= Constant.INFECTED_RATE_THRESHOLD_2)
                        {
                            Shielding(id, element.location, true, 60);
                            this.actionIds.Add(id);
                            id++;
                            SocialDistancing(id, element.location, Constant.RESTRICT_DISTANCE);
                            this.actionIds.Add(id);
                            id++;
                            MovementRestriction(id, element.location, Constant.RESTRICT_DISTANCE);
                            this.actionIds.Add(id);
                            id++;
                            StayAtHome(id, element.location);
                            this.actionIds.Add(id);
                            id++;
                            CloseSchool(id, element.location);
                            this.actionIds.Add(id);
                            id++;
                            CloseRecreational(id, element.location);
                            this.actionIds.Add(id);
                            id++;
                        }
                        else if(infectedRate >= Constant.INFECTED_RATE_THRESHOLD_2)
                        {
                            Shielding(id, element.location, true, 60);
                            this.actionIds.Add(id);
                            id++;
                            SocialDistancing(id, element.location, Constant.RESTRICT_DISTANCE);
                            this.actionIds.Add(id);
                            id++;
                            MovementRestriction(id, element.location, Constant.RESTRICT_DISTANCE);
                            this.actionIds.Add(id);
                            id++;
                            StayAtHome(id, element.location);
                            this.actionIds.Add(id);
                            id++;
                            CloseSchool(id, element.location);
                            this.actionIds.Add(id);
                            id++;
                            CloseRecreational(id, element.location);
                            this.actionIds.Add(id);
                            id++;
                            CloseBoder(id, element.location);
                            this.actionIds.Add(id);
                            id++;
                            Curfew(id, element.location);
                            this.actionIds.Add(id);
                            id++;
                        }
                    }
                }

                SendActionRequest();
                actions.Clear();

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
