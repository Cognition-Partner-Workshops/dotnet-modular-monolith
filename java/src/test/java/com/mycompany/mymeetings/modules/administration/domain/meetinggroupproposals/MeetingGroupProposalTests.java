package com.mycompany.mymeetings.modules.administration.domain.meetinggroupproposals;

import com.mycompany.mymeetings.buildingblocks.domain.BaseEntity;
import com.mycompany.mymeetings.buildingblocks.domain.BusinessRuleValidationException;
import com.mycompany.mymeetings.buildingblocks.domain.DomainEvent;
import com.mycompany.mymeetings.modules.administration.domain.meetinggroupproposals.events.MeetingGroupProposalAcceptedDomainEvent;
import com.mycompany.mymeetings.modules.administration.domain.meetinggroupproposals.events.MeetingGroupProposalRejectedDomainEvent;
import com.mycompany.mymeetings.modules.administration.domain.meetinggroupproposals.events.MeetingGroupProposalVerificationRequestedDomainEvent;
import com.mycompany.mymeetings.modules.administration.domain.meetinggroupproposals.rules.MeetingGroupProposalCanBeVerifiedOnceRule;
import com.mycompany.mymeetings.modules.administration.domain.meetinggroupproposals.rules.MeetingGroupProposalRejectionMustHaveAReasonRule;
import com.mycompany.mymeetings.modules.administration.domain.users.UserId;
import org.junit.jupiter.api.Test;

import java.time.LocalDateTime;
import java.util.List;
import java.util.UUID;

import static org.assertj.core.api.Assertions.assertThat;
import static org.assertj.core.api.Assertions.assertThatThrownBy;

class MeetingGroupProposalTests {

    @Test
    void createProposalToVerify_isSuccessful() {
        UUID meetingGroupProposalId = UUID.randomUUID();
        MeetingGroupLocation location = MeetingGroupLocation.create("Warsaw", "Poland");
        UserId proposalUserId = new UserId(UUID.randomUUID());
        LocalDateTime proposalDate = LocalDateTime.now();

        MeetingGroupProposal meetingGroupProposal = MeetingGroupProposal.createToVerify(
                meetingGroupProposalId,
                "meetingName",
                "meetingDescription",
                location,
                proposalUserId,
                proposalDate);

        MeetingGroupProposalVerificationRequestedDomainEvent event =
                assertPublishedDomainEvent(meetingGroupProposal, MeetingGroupProposalVerificationRequestedDomainEvent.class);
        assertThat(event.getMeetingGroupProposalId())
                .isEqualTo(new MeetingGroupProposalId(meetingGroupProposalId));
    }

    @Test
    void acceptProposal_whenDecisionIsNotMade_isSuccessful() {
        UUID meetingGroupProposalId = UUID.randomUUID();
        MeetingGroupLocation location = MeetingGroupLocation.create("Warsaw", "Poland");
        UserId proposalUserId = new UserId(UUID.randomUUID());
        LocalDateTime proposalDate = LocalDateTime.now();
        MeetingGroupProposal meetingGroupProposal = MeetingGroupProposal.createToVerify(
                meetingGroupProposalId,
                "meetingName",
                "meetingDescription",
                location,
                proposalUserId,
                proposalDate);

        meetingGroupProposal.accept(new UserId(UUID.randomUUID()));

        MeetingGroupProposalAcceptedDomainEvent event =
                assertPublishedDomainEvent(meetingGroupProposal, MeetingGroupProposalAcceptedDomainEvent.class);
        assertThat(event.getMeetingGroupProposalId())
                .isEqualTo(new MeetingGroupProposalId(meetingGroupProposalId));
    }

    @Test
    void acceptProposal_whenDecisionIsMade_canBeVerifiedOnlyOnce() {
        UUID meetingGroupProposalId = UUID.randomUUID();
        MeetingGroupLocation location = MeetingGroupLocation.create("Warsaw", "Poland");
        UserId userId = new UserId(UUID.randomUUID());
        LocalDateTime proposalDate = LocalDateTime.now();
        MeetingGroupProposal meetingGroupProposal = MeetingGroupProposal.createToVerify(
                meetingGroupProposalId,
                "meetingName",
                "meetingDescription",
                location,
                userId,
                proposalDate);

        meetingGroupProposal.accept(userId);

        assertBrokenRule(MeetingGroupProposalCanBeVerifiedOnceRule.class, () -> {
            meetingGroupProposal.accept(userId);
        });
    }

    @Test
    void rejectProposal_whenDecisionIsMade_canBeVerifiedOnlyOnce() {
        UUID meetingGroupProposalId = UUID.randomUUID();
        MeetingGroupLocation location = MeetingGroupLocation.create("Warsaw", "Poland");
        UserId userId = new UserId(UUID.randomUUID());
        LocalDateTime proposalDate = LocalDateTime.now();
        MeetingGroupProposal meetingGroupProposal = MeetingGroupProposal.createToVerify(
                meetingGroupProposalId,
                "meetingName",
                "meetingDescription",
                location,
                userId,
                proposalDate);

        meetingGroupProposal.accept(userId);

        assertBrokenRule(MeetingGroupProposalCanBeVerifiedOnceRule.class, () -> {
            meetingGroupProposal.reject(userId, "rejectReason");
        });
    }

    @Test
    void rejectProposal_withoutProvidedReason_cannotBeRejected() {
        UUID meetingGroupProposalId = UUID.randomUUID();
        MeetingGroupLocation location = MeetingGroupLocation.create("Warsaw", "Poland");
        UserId userId = new UserId(UUID.randomUUID());
        LocalDateTime proposalDate = LocalDateTime.now();
        MeetingGroupProposal meetingGroupProposal = MeetingGroupProposal.createToVerify(
                meetingGroupProposalId,
                "meetingName",
                "meetingDescription",
                location,
                userId,
                proposalDate);

        assertBrokenRule(MeetingGroupProposalRejectionMustHaveAReasonRule.class, () -> {
            meetingGroupProposal.reject(userId, "");
        });
    }

    @SuppressWarnings("unchecked")
    private <T extends DomainEvent> T assertPublishedDomainEvent(BaseEntity entity, Class<T> eventType) {
        List<DomainEvent> domainEvents = entity.getDomainEvents();
        assertThat(domainEvents).isNotEmpty();
        DomainEvent event = domainEvents.stream()
                .filter(eventType::isInstance)
                .findFirst()
                .orElse(null);
        assertThat(event).isNotNull().isInstanceOf(eventType);
        return (T) event;
    }

    private void assertBrokenRule(Class<?> ruleType, Runnable action) {
        assertThatThrownBy(action::run)
                .isInstanceOf(BusinessRuleValidationException.class)
                .satisfies(ex -> {
                    BusinessRuleValidationException brokenRuleEx = (BusinessRuleValidationException) ex;
                    assertThat(brokenRuleEx.getBrokenRule()).isInstanceOf(ruleType);
                });
    }
}
