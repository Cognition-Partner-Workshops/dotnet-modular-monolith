# MyMeetings API Documentation

## Architecture Overview

MyMeetings is a **.NET modular monolith** application that manages meeting groups, meetings, user registrations, and payments. The system is composed of independent modules that communicate through well-defined boundaries while sharing a single deployment unit.

### Modular Monolith Pattern

The application is organized into four business modules, each with its own **Application**, **Domain**, and **Infrastructure** layers:

| Module | Responsibility |
|---|---|
| **Administration** | Back-office operations such as reviewing and accepting meeting group proposals |
| **Meetings** | Core domain: meeting groups, meetings, attendees, comments, and country reference data |
| **Payments** | Subscription management, meeting fee payments, and price list administration |
| **UserAccess** | User registration, authentication, and authorization |

Shared infrastructure lives in the **BuildingBlocks** project, which provides cross-cutting concerns such as event handling, domain primitives, and infrastructure contracts.

### CQRS (Command Query Responsibility Segregation)

Each module follows the CQRS pattern:

- **Commands** mutate state and are dispatched through `IModule.ExecuteCommandAsync`.
- **Queries** read state and are dispatched through `IModule.ExecuteQueryAsync`.

Controllers act as thin HTTP adapters that translate incoming requests into commands or queries and delegate execution to the appropriate module contract (`IAdministrationModule`, `IMeetingsModule`, `IPaymentsModule`, `IUserAccessModule`, `IRegistrationsModule`).

### Dependency Injection

Modules are registered and configured using **Autofac** containers, enabling each module to have its own composition root while still sharing the ASP.NET Core host.

---

## Authentication & Authorization

The API uses **JWT Bearer tokens** for authentication. Most endpoints require the caller to be authenticated.

### Permission-Based Authorization

Authorization is enforced at the action level using a custom `[HasPermission("PermissionName")]` attribute. Each module defines its own set of permission constants (e.g., `MeetingsPermissions`, `PaymentsPermissions`, `AdministrationPermissions`). The authenticated user's permissions are checked before the action executes.

Some endpoints are marked with `[NoPermissionRequired]` and/or `[AllowAnonymous]`, indicating they are accessible without specific permissions or without authentication at all (e.g., user registration and email retrieval).

---

## API Endpoints

All endpoints return JSON. Request bodies are expected as `application/json` unless otherwise noted.

### Administration Module

Base path: `api/administration`

| Method | Path | Description | Permission | Request Body | Response |
|---|---|---|---|---|---|
| `GET` | `/meetingGroupProposals` | List all meeting group proposals | `AcceptMeetingGroupProposal` | - | `MeetingGroupProposalDto[]` |
| `PATCH` | `/meetingGroupProposals/{meetingGroupProposalId}/accept` | Accept a meeting group proposal | `AcceptMeetingGroupProposal` | - | `200 OK` |

---

### Meetings Module

#### Countries

Base path: `api/meetings/countries`

| Method | Path | Description | Permission | Request Body / Query | Response |
|---|---|---|---|---|---|
| `GET` | `/` | Get all countries (paginated) | None required | `?page={int}&perPage={int}` | `CountryDto[]` |

#### Meeting Group Proposals

Base path: `api/meetings/meetingGroupProposals`

| Method | Path | Description | Permission | Request Body | Response |
|---|---|---|---|---|---|
| `GET` | `/` | Get the authenticated member's meeting group proposals | `GetMeetingGroupProposals` | - | `MeetingGroupProposalDto[]` |
| `GET` | `/all` | Get all meeting group proposals | `GetMeetingGroupProposals` | - | `MeetingGroupProposalDto[]` |
| `POST` | `/` | Propose a new meeting group | `ProposeMeetingGroup` | `ProposeMeetingGroupRequest` | `200 OK` |

**ProposeMeetingGroupRequest**

```json
{
  "name": "string",
  "description": "string",
  "locationCity": "string",
  "locationCountryCode": "string"
}
```

#### Meeting Groups

Base path: `api/meetings/meetingGroups`

| Method | Path | Description | Permission | Request Body | Response |
|---|---|---|---|---|---|
| `GET` | `/` | Get the authenticated member's meeting groups | `GetAuthenticatedMemberMeetingGroups` | - | `MemberMeetingGroupDto[]` |
| `GET` | `/{meetingGroupId}` | Get meeting group details | `GetMeetingGroupDetails` | - | `MeetingGroupDetailsDto` |
| `GET` | `/all` | Get all meeting groups | `GetAllMeetingGroups` | - | `MeetingGroupDto[]` |
| `PUT` | `/{meetingGroupId}` | Edit meeting group general attributes | `EditMeetingGroupGeneralAttributes` | `EditMeetingGroupGeneralAttributesRequest` | `200 OK` |
| `POST` | `/{meetingGroupId}/members` | Join a meeting group | `JoinToGroup` | - | `200 OK` |
| `DELETE` | `/{meetingGroupId}/members` | Leave a meeting group | `LeaveMeetingGroup` | - | `200 OK` |

**EditMeetingGroupGeneralAttributesRequest**

```json
{
  "meetingGroupId": "guid",
  "name": "string",
  "description": "string",
  "locationCity": "string",
  "locationCountryCode": "string"
}
```

#### Meetings

Base path: `api/meetings/meetings`

| Method | Path | Description | Permission | Request Body | Response |
|---|---|---|---|---|---|
| `GET` | `/` | Get the authenticated member's meetings | `GetAuthenticatedMemberMeetings` | - | `MemberMeetingDto[]` |
| `GET` | `/{meetingId}` | Get meeting details | `GetMeetingDetails` | - | `MeetingDetailsDto` |
| `POST` | `/` | Create a new meeting | `CreateNewMeeting` | `CreateMeetingRequest` | `200 OK` |
| `PUT` | `/{meetingId}` | Edit a meeting | `EditMeeting` | `ChangeMeetingMainAttributesRequest` | `200 OK` |
| `GET` | `/{meetingId}/attendees` | Get meeting attendees | `GetMeetingAttendees` | - | `MeetingAttendeeDto[]` |
| `POST` | `/{meetingId}/attendees` | Add an attendee to a meeting | `AddMeetingAttendee` | `AddMeetingAttendeeRequest` | `200 OK` |
| `DELETE` | `/{meetingId}/attendees` | Remove an attendee from a meeting | `RemoveMeetingAttendee` | `RemoveMeetingAttendeeRequest` | `200 OK` |
| `POST` | `/{meetingId}/notAttendees` | Mark a member as not attending | `AddNotAttendee` | - | `200 OK` |
| `PATCH` | `/{meetingId}/notAttendees` | Change a not-attending decision | `ChangeNotAttendeeDecision` | - | `200 OK` |
| `POST` | `/{meetingId}/waitlistMembers` | Sign up member to waitlist | `SignUpMemberToWaitlist` | - | `200 OK` |
| `DELETE` | `/{meetingId}/waitlistMembers` | Sign off member from waitlist | `SignOffMemberFromWaitlist` | - | `200 OK` |
| `POST` | `/{meetingId}/hosts` | Set meeting host role | `SetMeetingHostRole` | `SetMeetingHostRequest` | `200 OK` |
| `DELETE` | `/{meetingId}/hosts` | Set meeting attendee role (demote host) | `SetMeetingAttendeeRole` | `SetMeetingAttendeeRequest` | `200 OK` |
| `PATCH` | `/{meetingId}/cancel` | Cancel a meeting | `CancelMeeting` | - | `200 OK` |

**CreateMeetingRequest**

```json
{
  "meetingGroupId": "guid",
  "title": "string",
  "termStartDate": "datetime",
  "termEndDate": "datetime",
  "description": "string",
  "meetingLocationName": "string",
  "meetingLocationAddress": "string",
  "meetingLocationPostalCode": "string",
  "meetingLocationCity": "string",
  "attendeesLimit": "int | null",
  "guestsLimit": "int",
  "rsvpTermStartDate": "datetime | null",
  "rsvpTermEndDate": "datetime | null",
  "eventFeeValue": "decimal | null",
  "eventFeeCurrency": "string | null",
  "hostMemberIds": ["guid"]
}
```

**ChangeMeetingMainAttributesRequest**

```json
{
  "meetingGroupId": "guid",
  "title": "string",
  "termStartDate": "datetime",
  "termEndDate": "datetime",
  "description": "string",
  "meetingLocationName": "string",
  "meetingLocationAddress": "string",
  "meetingLocationPostalCode": "string",
  "meetingLocationCity": "string",
  "attendeesLimit": "int | null",
  "guestsLimit": "int",
  "rsvpTermStartDate": "datetime | null",
  "rsvpTermEndDate": "datetime | null",
  "eventFeeValue": "decimal | null",
  "eventFeeCurrency": "string | null"
}
```

**AddMeetingAttendeeRequest** / **RemoveMeetingAttendeeRequest** / **SetMeetingHostRequest** / **SetMeetingAttendeeRequest**

```json
{
  "memberId": "guid"
}
```

#### Meeting Comments

Base path: `api/meetings/meetings/{meetingId}/comments`

| Method | Path | Description | Permission | Request Body | Response |
|---|---|---|---|---|---|
| `POST` | `/` | Add a comment to a meeting | `AddMeetingComment` | `AddMeetingCommentRequest` | `200 OK` |
| `PUT` | `/{meetingCommentId}` | Edit a meeting comment | `EditMeetingComment` | `EditMeetingCommentRequest` | `200 OK` |
| `DELETE` | `/{meetingCommentId}` | Delete a meeting comment | `RemoveMeetingComment` | - | `200 OK` |
| `POST` | `/{meetingCommentId}/replies` | Reply to a meeting comment | `AddMeetingCommentReply` | `AddMeetingCommentRequest` | `200 OK` |
| `POST` | `/{meetingCommentId}/likes` | Like a meeting comment | `LikeMeetingComment` | - | `200 OK` |
| `DELETE` | `/{meetingCommentId}/likes` | Unlike a meeting comment | `UnlikeMeetingComment` | - | `200 OK` |

**AddMeetingCommentRequest** / **EditMeetingCommentRequest**

```json
{
  "comment": "string"
}
```

#### Meeting Commenting Configuration

Base path: `api/meetings/meetings/{meetingId}/commentingConfiguration`

| Method | Path | Description | Permission | Request Body | Response |
|---|---|---|---|---|---|
| `PATCH` | `/disable` | Disable commenting on a meeting | `DisableMeetingCommenting` | - | `200 OK` |
| `PATCH` | `/enable` | Enable commenting on a meeting | `EnableMeetingCommenting` | - | `200 OK` |

---

### Payments Module

#### Meeting Fee Payments

Base path: `api/payments/meetingFeePayments`

| Method | Path | Description | Permission | Request Body | Response |
|---|---|---|---|---|---|
| `POST` | `/` | Create a meeting fee payment | `RegisterPayment` | `CreateMeetingFeePaymentRequest` | `200 OK` |
| `PUT` | `/{meetingFeePaymentId}/purchased` | Mark a meeting fee payment as paid | `RegisterPayment` | - | `200 OK` |

**CreateMeetingFeePaymentRequest**

```json
{
  "meetingFeeId": "guid"
}
```

#### Payers

Base path: `api/payments/payers`

| Method | Path | Description | Permission | Request Body | Response |
|---|---|---|---|---|---|
| `GET` | `/authenticated/subscription` | Get the authenticated payer's subscription | `GetAuthenticatedPayerSubscription` | - | `SubscriptionDetailsDto` |

#### Price List Items

Base path: `api/payments/priceListItems`

| Method | Path | Description | Permission | Request Body / Query | Response |
|---|---|---|---|---|---|
| `GET` | `/` | Get a price list item by filter | `GetPriceListItem` | `?countryCode={string}&categoryCode={string}&periodTypeCode={string}` | `PriceListItemMoneyValueDto` |
| `POST` | `/` | Create a new price list item | `CreatePriceListItem` | `CreatePriceListItemRequest` | `200 OK` |
| `PATCH` | `/{priceListItemId}/activate` | Activate a price list item | `ActivatePriceListItem` | - | `200 OK` |
| `PATCH` | `/{priceListItemId}/deactivate` | Deactivate a price list item | `DeactivatePriceListItem` | - | `200 OK` |
| `PUT` | `/` | Change price list item attributes | `ChangePriceListItemAttributes` | `ChangePriceListItemAttributesRequest` | `200 OK` |

**CreatePriceListItemRequest**

```json
{
  "countryCode": "string",
  "subscriptionPeriodCode": "string",
  "categoryCode": "string",
  "priceValue": "decimal",
  "priceCurrency": "string"
}
```

**ChangePriceListItemAttributesRequest**

```json
{
  "priceListItemId": "guid",
  "countryCode": "string",
  "subscriptionPeriodCode": "string",
  "categoryCode": "string",
  "priceValue": "decimal",
  "priceCurrency": "string"
}
```

#### Subscriptions

Base path: `api/payments/subscriptions`

| Method | Path | Description | Permission | Request Body | Response |
|---|---|---|---|---|---|
| `POST` | `/` | Buy a subscription | `BuySubscription` | `BuySubscriptionRequest` | Payment ID (`Guid`) |
| `POST` | `/{subscriptionId}/renewals` | Renew a subscription | `RenewSubscription` | `RenewSubscriptionRequest` | `202 Accepted` |

**BuySubscriptionRequest** / **RenewSubscriptionRequest**

```json
{
  "subscriptionTypeCode": "string",
  "countryCode": "string",
  "value": "decimal",
  "currency": "string"
}
```

#### Subscription Payments

Base path: `api/payments/subscriptionPayments`

| Method | Path | Description | Permission | Request Body | Response |
|---|---|---|---|---|---|
| `POST` | `/` | Mark a subscription payment as paid | `RegisterPayment` | `RegisterSubscriptionPaymentRequest` | `200 OK` |

**RegisterSubscriptionPaymentRequest**

```json
{
  "paymentId": "guid"
}
```

#### Subscription Renewals

Base path: `api/payments/subscriptionRenewals`

| Method | Path | Description | Permission | Request Body | Response |
|---|---|---|---|---|---|
| `POST` | `/` | Mark a subscription renewal payment as paid | `RegisterPayment` | `RegisterSubscriptionRenewalPaymentRequest` | `200 OK` |

**RegisterSubscriptionRenewalPaymentRequest**

```json
{
  "paymentId": "guid"
}
```

---

### UserAccess Module

#### Authenticated User

Base path: `api/userAccess/authenticatedUser`

| Method | Path | Description | Permission | Request Body | Response |
|---|---|---|---|---|---|
| `GET` | `/` | Get the authenticated user's profile | None required | - | `UserDto` |
| `GET` | `/permissions` | Get the authenticated user's permissions | None required | - | `UserPermissionDto[]` |

#### User Registrations

Base path: `userAccess/UserRegistrations`

| Method | Path | Description | Auth Required | Request Body | Response |
|---|---|---|---|---|---|
| `POST` | `/` | Register a new user | No (anonymous) | `RegisterNewUserRequest` | `200 OK` |
| `PATCH` | `/{userRegistrationId}/confirm` | Confirm a user registration | No (anonymous) | - | `200 OK` |

**RegisterNewUserRequest**

```json
{
  "login": "string",
  "password": "string",
  "email": "string",
  "firstName": "string",
  "lastName": "string",
  "confirmLink": "string"
}
```

#### Emails

Base path: `api/userAccess/emails`

| Method | Path | Description | Auth Required | Request Body | Response |
|---|---|---|---|---|---|
| `GET` | `/` | Get all system emails | No (anonymous) | - | `EmailDto[]` |

---

## Module Responsibilities & Boundaries

### Administration

Handles back-office administrative operations. Currently responsible for reviewing and accepting meeting group proposals submitted through the Meetings module. Administrators can list pending proposals and accept them, which triggers the creation of the corresponding meeting group.

**Key permission:** `AcceptMeetingGroupProposal`

### Meetings

The core domain module. Manages the complete lifecycle of meeting groups and meetings:

- **Meeting Groups**: Creation (via proposals), membership (join/leave), and attribute management.
- **Meetings**: Creation within groups, editing, cancellation, attendee management (add/remove/waitlist), role assignment (host/attendee), and not-attending declarations.
- **Comments**: Full comment lifecycle on meetings including replies, likes, and commenting configuration (enable/disable).
- **Countries**: Reference data for location selection.

### Payments

Manages the financial aspects of the platform:

- **Subscriptions**: Purchasing and renewing user subscriptions with configurable pricing.
- **Meeting Fees**: Creating and registering payments for meeting attendance fees.
- **Price List**: CRUD operations for subscription pricing by country, category, and period.

### UserAccess

Manages user identity and access control:

- **Registration**: Self-service user sign-up with email confirmation (anonymous access).
- **Authentication**: JWT-based user authentication.
- **Authorization**: Permission retrieval for the authenticated user.
- **Emails**: System email retrieval for debugging/monitoring purposes.
