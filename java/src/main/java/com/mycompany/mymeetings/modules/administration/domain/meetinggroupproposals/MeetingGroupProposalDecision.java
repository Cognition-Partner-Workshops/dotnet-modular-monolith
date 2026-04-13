package com.mycompany.mymeetings.modules.administration.domain.meetinggroupproposals;

import com.mycompany.mymeetings.buildingblocks.domain.ValueObject;
import com.mycompany.mymeetings.modules.administration.domain.users.UserId;

import java.time.LocalDateTime;
import java.util.UUID;

public class MeetingGroupProposalDecision extends ValueObject {

    private final LocalDateTime date;
    private final UserId userId;
    private final String code;
    private final String rejectReason;

    private MeetingGroupProposalDecision(LocalDateTime date, UserId userId, String code, String rejectReason) {
        this.date = date;
        this.userId = userId;
        this.code = code;
        this.rejectReason = rejectReason;
    }

    public static MeetingGroupProposalDecision noDecision() {
        return new MeetingGroupProposalDecision(null, null, null, null);
    }

    public static MeetingGroupProposalDecision acceptDecision(LocalDateTime date, UserId userId) {
        return new MeetingGroupProposalDecision(date, userId, "Accept", null);
    }

    public static MeetingGroupProposalDecision rejectDecision(LocalDateTime date, UserId userId, String rejectReason) {
        return new MeetingGroupProposalDecision(date, userId, "Reject", rejectReason);
    }

    MeetingGroupProposalStatus getStatusForDecision() {
        if (isAccepted()) {
            return MeetingGroupProposalStatus.verified();
        }
        if (isRejected()) {
            return MeetingGroupProposalStatus.create("Rejected");
        }
        return MeetingGroupProposalStatus.toVerify();
    }

    private boolean isAccepted() {
        return "Accept".equals(this.code);
    }

    private boolean isRejected() {
        return "Reject".equals(this.code);
    }

    public LocalDateTime getDate() {
        return date;
    }

    public UserId getUserId() {
        return userId;
    }

    public String getCode() {
        return code;
    }

    public String getRejectReason() {
        return rejectReason;
    }
}
