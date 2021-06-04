using System;
using System.Collections.Generic;

namespace Simulation {

    class Game {
        private Random rand = new Random(0);
        private Person[] people;
        internal List<Location> locations;
        internal Dictionary<Location, List<Person>> clocLUT, hlocLUT;
        private uint step = 0;
        private uint step_size = 1;

        public Game(uint popSize) {
            people = new Person[popSize];
            locations = new List<Location>();
            clocLUT = new Dictionary<Location, List<Person>>();
            hlocLUT = new Dictionary<Location, List<Person>>();

            // TODO: Replace this dummy initialisation
            for (uint i = 0; i < (popSize/5); i++) {
                Location hl = new Location(0.0, 0.0);
                locations.Add(hl);
                people[5*i] = new Person(5*i, "Person "+(5*i), hl);
                people[5*i+1] = new Person(5*i+1, "Person "+(5*i+1), hl);
                people[5*i+2] = new Person(5*i+2, "Person "+(5*i+2), hl);
                people[5*i+3] = new Person(5*i+3, "Person "+(5*i+3), hl);
                people[5*i+4] = new Person(5*i+4, "Person "+(5*i+4), hl);

                var locList1 = new List<Person>();

                locList1.Add(people[5*i]);
                locList1.Add(people[5*i+1]);
                locList1.Add(people[5*i+2]);
                locList1.Add(people[5*i+3]);
                locList1.Add(people[5*i+4]);

                var locList2 = new List<Person>(locList1);

                clocLUT[hl] = locList1;
                hlocLUT[hl] = locList2;
            }

             // Make a random person patient zero
             people[rand.Next((int) popSize)].patient0();
        }

        // TODO: Implement me
        public void registerWHOAction(Object action) {
        }

        public void doGameStep() {
            // Process each person
            foreach(Person p in people) {
                if (!p.isDead()) {
                    p.doStep(this);
                }
            }

            // Handle infections
            foreach (Location location in locations) {
                // TODO: Optimise (keep LUT?)
                foreach(Person p in clocLUT[location]) {
                    if (p.isInfectious()) {
                        Virus contaigon = p.getLastInfectedWith();
                        foreach(Person p2 in clocLUT[location]) {
                            p2.expose(contaigon);
                        }
                    }
                }
            }

            this.step += this.step_size;
        }

        public void setStepSize(uint step_size) {
            this.step_size = step_size;
        }

        public IReadOnlyList<Person> getAllPeople() {
            return Array.AsReadOnly(people);
        }
    }

    class Person {
        public enum InfectionStatus {
            Uninfected,
            AsympAInf,
            AsympInf,
            SympInf,
            SevereSymp,
            Dead,
            Immune
        }

        private uint id;
        private string name;
        private InfectionStatus infectionStatus;
        private Location homeLocation;
        private Location currentLocation;
        private Virus lastInfectedWith;
        private Random rand;

        public Person(uint id, string name, Location homeLocation) {
            this.id = id;
            this.name = name;
            this.infectionStatus = InfectionStatus.Uninfected;
            this.homeLocation = homeLocation;
            this.currentLocation = homeLocation;

            // TODO: Do we need to provida a seed here?
            this.rand = new Random();
        }

        public void doStep(Game game) {
            // Handle infection progression
            switch (infectionStatus) {
                // No action on uninfected
                case InfectionStatus.Uninfected:
                    break;
                // Infection can progress or end
                case InfectionStatus.AsympAInf:
                case InfectionStatus.AsympInf:
                case InfectionStatus.SympInf:
                case InfectionStatus.SevereSymp:
                    if (rand.NextDouble() < lastInfectedWith.getRecoveryChance(this)) {
                        infectionStatus = InfectionStatus.Immune;
                    }
                    else if (rand.NextDouble() < lastInfectedWith.getProgressionChance(this)) {
                        infectionStatus++;
                    }
                    break;
                // No action on dead
                case InfectionStatus.Dead:
                    break;
                // Immunity can be lost with probability
                case InfectionStatus.Immune:
                    if (rand.NextDouble() < lastInfectedWith.getImmunityLossChance(this)) {
                        infectionStatus = InfectionStatus.Uninfected;
                    }
                    break;
            }

            // Handle movement
            // TODO: Implement some actual decision logic here
            Location newLocation = game.locations[rand.Next((int) game.locations.Count)];

            if (newLocation != currentLocation) {
                game.clocLUT[currentLocation].Remove(this);
                game.clocLUT[newLocation].Add(this);
                currentLocation = newLocation;
            }
        }

        public bool isDead() {
            return infectionStatus == InfectionStatus.Dead;
        }

        public bool isInfected() {
            switch (infectionStatus) {
                case InfectionStatus.AsympAInf:
                case InfectionStatus.AsympInf:
                case InfectionStatus.SympInf:
                case InfectionStatus.SevereSymp:
                    return true;
            }
            return false;
        }

        public bool isInfectious() {
            switch (infectionStatus) {
                case InfectionStatus.AsympInf:
                case InfectionStatus.SympInf:
                case InfectionStatus.SevereSymp:
                    return true;
            }
            return false;
        }

        public void expose(Virus v) {
            if (!(this.isInfected()) && (rand.NextDouble() < v.getInfectionChance(this))) {
                infectionStatus = InfectionStatus.AsympAInf;
                lastInfectedWith = v;
            }
        }

        public void patient0() {
            infectionStatus = InfectionStatus.AsympInf;
            lastInfectedWith = new Virus();
        }

        public Virus getLastInfectedWith() {
            return lastInfectedWith;
        }
    }

    class Location {
        private double x, y;

        public Location(double x, double y) {
            this.x = x;
            this.y = y;
        }

        public double distanceTo(Location other) {
            return Math.Sqrt(Math.Pow(x-other.x, 2) + Math.Pow(y-other.y, 2));
        }

        public (double x, double y) coordinates() {
            return (x, y);
        }

    }

    class Virus {
        // TODO: These shouldn't be static, but dependant on the person's risk factors
        public double getRecoveryChance(Person p) {
            return 0.1;
        }
        public double getProgressionChance(Person p) {
            return 0.1;
        }
        public double getImmunityLossChance(Person p) {
            return 0.1;
        }
        public double getInfectionChance(Person p) {
            return 0.1;
        }
    }
}
