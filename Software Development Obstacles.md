# Software Development Obstacles

---

![[Software Development Obstacles Image.png]]

---

## Introduction and Purpose

Highlight current challenges and key findings and recommendations for improving the software development environment. This is based on my observations during the past year working with the team. Many of these should seem obvious, but they have nevertheless not been addressed.

---

## Issue Categories

- Platform Reliability
- Service Observability
- Absent Management
- Resources & Workload
- Team Autonomy
- Unclear Requirement

---

## Glossary

**Platform**. Services and systems that the team relies on to operate but are not responsible for maintaining, e.g. gitlab, jenkins, databases, splunk.

---

## General Consequences

- Low team morale and cohesion.
- Increased risk of losing developers.
- Frequent interruptions and delays in development.
- Increased downtime and lost productivity.
- Reactive troubleshooting.

---

## Resources & Workload

The development team has reduced from seven members, including myself, to four, with only two being internal employees who have been with the company for over five years, posing a significant flight risk.

Despite an increasing workload with an expanding domain and systems portfolio, the team's resources remain unchanged. Requests for platform enhancements are met with skepticism regarding cost and necessity.

#flight-risks and impact if we lose more devs.

The team is overburdened, with no indication of additional resources or budget allocation to address the loss of four developers over the past year. Expectations to deliver more with significantly fewer resources remain unaddressed.

#deadlines what are they and set by who?

#domains hvilke domæner taler vi om?

---

**Examples**

- Number of parks increase.
- Lost three devs this year.
- Only **two** internal devs remaining.

---

**Consequences**

- Knowledge sharing is limited due to lack of time.
- Hacky solutions are often chosen to meet deadlines.
- Developers are often interrupted by other departments. #fix

---

**Recommendation**

- Prefer internal hires over contractors to build domain knowledge.
- Accept lower relative productivity to encourage knowledge sharing.
- Identify core domain and remove anything non-essential.
- ~~Hire new developers to reduce the workload on the current team.~~

TH: 3m
SU: 6m
EM:

---

## Absent Management

Management seems overburdened and disconnected from team. Most major decisions are made without consulting the team or entirely left to the team without guidance or support from management. This has led to a lack of direction and focus for the team.

---

**Examples**

- Re-org and the new department merges.
- Splunk deprecated without notice.
- No operational support from organisation for maintaining platform. #clarify

---

**Consequences**

- General employee concerns are not addressed.

---

**Recommendation**

- Physically present manager, preferably in-office.
- Managers should participate in team sprint review.
- Support team in non-reactionary handling of tasks e.g. *Their lack of planning is not our emergency*.

---

## Unclear Requirements

The team often receives requirements that are incomplete or unclear. Due to the current workload and expected pace of development, the team often have to make assumptions or decisions without full picture. Requirements will also conflict with each other. The current domain also simply too large for the team to handle. Often is is also unclear which tasks are the most important - leaving the developers to guess and then be blamed for making the wrong choice.

#priority - hvad mente vi?

#todo the stakeholders pay to develop a feature, but once done it's somehow only a cost to the team, and other departments can piggyback and get free additions.

---

**Examples**

- Missing error codes during Blauw onboarding.
- Park onboarding
- Should OTA support hybrid parks?
- Will WM be deprecated soon?

---

**Consequences**

- Developers act as project managers - increasing context switching.
- Implementation starts before requirements are clear.

---

**Recommendation**

- Establish a clear and concise process for gathering and persisting requirements.
- Support team in not starting implementation before requirements are clear.
- No tasks should have the same priority - developers should not have to guess.

---

## Platform Reliability

The team relies on several externally managed systems to operate effectively. These include version control (gitlab), build servers (jenkins), databases, package management (artifactory), virtual machines, and servers. Many of these systems are provided with only basic functionality, often only offering a restrictive community version.

#basic-need

These systems are frequently unavailable during regular operation, this includes both planned and unplanned downtime. The team has no control over these systems but are held accountable for their performance. Constant interuptions are caused by the development virtual machines being are slow and the network is unreliable.

The team responsible for maintaining these machines are either unable or unwilling to fix these issues.

---

**Examples**

- Daily database backups are fail often, blocking development.
- Virtual machines are slow and unresponsive.
- Working on virtual machine is like swimming in sirup.
- Windows 11 migration has blocked development (proxy, firewall, RDP).

---

**Consequences**

- Team is blamed for downtime and lost productivity.
- Frequent interruptions and severe delays in development.
- Severe decrease in developer productivity.
- Systems are only available with barebones functionality. #change
- Productivily benifits from tools are not realised.
- Developers avoid experimentation, update and upgrades.

---

**Recommendation**

- Replace the operations team.
- Use physical laptops for development.
- Developers must have administrative rights to their development machines.

---

## Service Observability

In order to maintain and be proactive, the team needs reliable monitoring and logging systems. The current systems are either inadequate or unavailable when it comes to the necessary information to identify and resolve issues. This leads to reactive troubleshooting and increased downtime. The departments responsible for these systems have not prioritized improvements.

#has-access only and not team.

Whenever the log retention is discussed, the cost of harddisks are brought up. We currently ingest ~10 GB per day. For reference, a 1 TB HDD costs approx 400 DKK.

---

**Examples**

- Inconsistent and arbitrary log retention policy.
- Efforts to implement observability tools have been ignored or deprioritized.
- Service logs are arbitrarily deleted - *"the disk was running low"*.
- Splunk potential not realised.

---

**Consequences**

- Increased downtime and lost productivity.
- Difficulty troubleshooting issues.
- Reactive troubleshooting and maintenance.

---

**Recommendation**

- Establish or find observability platform used by other teams in the company.
- Switch to expert provider to run and maintain this platform.

---

## Team Autonomy

The teams lacks autonomy. During almost every step of development they rely on other departments to complete their work. This includes frequent configuration of DTAP environments, deployment of applications, troubleshooting and accessing logs. This leads to delays and increased downtime. This negatively impacts morale, reduces productivity and increases the risk of losing developers.

---

**Examples**

- Database changes require interaction with HAS.
- Build servers can't be changed without HAS.
- Database for tests must be daily blessed by HAS #how?
- Production logs are only available to HAS #validate.
- #more-examples.

---

**Consequences**

- Constant interuptions and context switches.
- Developers become intermediate project managers.

---

 **Recommendation**

- The team must be in control of the entire development workflow to reduce friction and delays.
- Combine departments and place them in same physical location.
- Account for Conway's Law.

---

## Office Space and Work Stations

The team is currently working in an open office space with free seating. This is not optimal for software teams. The team needs a dedicated space where they can focus and collaborate without being disturbed. The team can't coorperate if they are not in the same physical location. Having to redo the seating puzzle every morning is a huge waste of time.

---

**Examples**

#todo

---

**Consequences**

#todo

---

**Recommendation**

- Provide the team with a dedicated office space.

---

## On-boarding

The organisation has no on-boarding process for new developers. Everything is hand held by the current team and single individuals.

## Other Points

Other points to include.

#todo team product udvikling cost

- The team should be given mandate where they have responsibility.
- Costs seem to always end up with the team. Not with HAS or other departments.
- Previous Wind Manager 2 project failed due to several other reasons than those mentioned in the report.
- Office space and work stations with free seating are not optimal for software teams.
- No other software teams in the company to share knowledge with.
- Team has responsibility for systems, but not the mandate to make changes.
