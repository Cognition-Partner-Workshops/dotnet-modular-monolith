package com.mycompany.mymeetings.modules.administration.domain.meetinggroupproposals;

import com.mycompany.mymeetings.buildingblocks.domain.AggregateRoot;
import com.mycompany.mymeetings.buildingblocks.domain.BaseEntity;
import com.mycompany.mymeetings.modules.administration.domain.meetinggroupproposals.events.MeetingGroupProposalAcceptedDomainEvent;
import com.mycompany.mymeetings.modules.administration.domain.meetinggroupproposals.events.MeetingGroupProposalRejectedDomainEvent;
import com.mycompany.mymeetings.modules.administration.domain.meetinggroupproposals.events.MeetingGroupProposalVerificationRequestedDomainEvent;
import com.mycompany.mymeetings.modules.administration.domain.meetinggroupproposals.rules.MeetingGroupProposalCanBeVerifiedOnceRule;
import com.mycompany.mymeetings.modules.administration.domain.meetinggroupproposals.rules.MeetingGroupProposalRejectionMustHaveAReasonRule;
import com.mycompany.mymeetings.modules.administration.domain.users.UserId;
import jakarta.persistence.AttributeOverride;
import jakarta.persistence.AttributeOverrides;
import jakarta.persistence.Column;
import jakarta.persistence.Embedded;
import jakarta.persistence.Entity;
import jakarta.persistence.Id;
import jakarta.persistence.Table;
import jakarta.persistence.Transient;

import java.time.LocalDateTime;
import java.util.UUID;

@Entity
@Table(name = "MeetingGroupProposals", schema = "administration")
public class MeetingGroupProposal extends BaseEntity implements AggregateRoot {

    @Id
    @Column(name = "Id")
    private UUID id;

    @Column(name = "Name")
    private String name;

    @Column(name = "Description")
    private String description;

    @Embedded
    @AttributeOverrides({
        @AttributeOverride(name = "city", column = @Column(name = "LocationCity")),
        @AttributeOverride(name = "countryCode", column = @Column(name = "LocationCountryCode"))
    })
    private MeetingGroupLocation location;

    @Column(name = "ProposalDate")
    private LocalDateTime proposalDate;

    @Column(name = "ProposalUserId")
    private UUID proposalUserId;

    @Column(name = "StatusCode")
    private String statusCode;

    @Column(name = "DecisionCode")
    private String decisionCode;

    @Column(name = "DecisionDate")
    private LocalDateTime decisionDate;

    @Column(name = "DecisionUserId")
    private UUID decisionUserId;

    @Column(name = "DecisionRejectReason")
    private String decisionRejectReason;

    @Transient
    private MeetingGroupProposalStatus status;

    @Transient
    private MeetingGroupProposalDecision decision;

    protected MeetingGroupProposal() {
        this.decision = MeetingGroupProposalDecision.noDecision();
    }

    private MeetingGroupProposal(
            MeetingGroupProposalId id,
            String name,
            String description,
            MeetingGroupLocation location,
            UserId proposalUserId,
            LocalDateTime proposalDate) {
        this.id = id.getValue();
        this.name = name;
        this.description = description;
        this.location = location;
        this.proposalUserId = proposalUserId.getValue();
        this.proposalDate = proposalDate;

        this.status = MeetingGroupProposalStatus.toVerify();
        this.statusCode = this.status.getValue();
        this.decision = MeetingGroupProposalDecision.noDecision();

        this.addDomainEvent(new MeetingGroupProposalVerificationRequestedDomainEvent(getId()));
    }

    public MeetingGroupProposalId getId() {
        return new MeetingGroupProposalId(id);
    }

    public void accept(UserId userId) {
        this.checkRule(new MeetingGroupProposalCanBeVerifiedOnceRule(getDecision()));

        this.decision = MeetingGroupProposalDecision.acceptDecision(LocalDateTime.now(), userId);
        this.status = this.decision.getStatusForDecision();

        this.decisionCode = this.decision.getCode();
        this.decisionDate = this.decision.getDate();
        this.decisionUserId = userId.getValue();
        this.statusCode = this.status.getValue();

        this.addDomainEvent(new MeetingGroupProposalAcceptedDomainEvent(getId()));
    }

    public void reject(UserId userId, String rejectReason) {
        this.checkRule(new MeetingGroupProposalCanBeVerifiedOnceRule(getDecision()));
        this.checkRule(new MeetingGroupProposalRejectionMustHaveAReasonRule(rejectReason));

        this.decision = MeetingGroupProposalDecision.rejectDecision(LocalDateTime.now(), userId, rejectReason);
        this.status = this.decision.getStatusForDecision();

        this.decisionCode = this.decision.getCode();
        this.decisionDate = this.decision.getDate();
        this.decisionUserId = userId.getValue();
        this.decisionRejectReason = rejectReason;
        this.statusCode = this.status.getValue();

        this.addDomainEvent(new MeetingGroupProposalRejectedDomainEvent(getId()));
    }

    public static MeetingGroupProposal createToVerify(
            UUID meetingGroupProposalId,
            String name,
            String description,
            MeetingGroupLocation location,
            UserId proposalUserId,
            LocalDateTime proposalDate) {
        return new MeetingGroupProposal(
                new MeetingGroupProposalId(meetingGroupProposalId),
                name,
                description,
                location,
                proposalUserId,
                proposalDate);
    }

    private MeetingGroupProposalDecision getDecision() {
        if (this.decision != null) {
            return this.decision;
        }
        if (this.decisionCode != null) {
            if ("Accept".equals(this.decisionCode)) {
                return MeetingGroupProposalDecision.acceptDecision(
                        this.decisionDate, new UserId(this.decisionUserId));
            } else if ("Reject".equals(this.decisionCode)) {
                return MeetingGroupProposalDecision.rejectDecision(
                        this.decisionDate, new UserId(this.decisionUserId), this.decisionRejectReason);
            }
        }
        return MeetingGroupProposalDecision.noDecision();
    }
}
