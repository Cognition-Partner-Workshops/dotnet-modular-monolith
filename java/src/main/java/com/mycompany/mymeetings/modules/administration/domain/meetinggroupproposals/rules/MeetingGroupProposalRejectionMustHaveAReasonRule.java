package com.mycompany.mymeetings.modules.administration.domain.meetinggroupproposals.rules;

import com.mycompany.mymeetings.buildingblocks.domain.BusinessRule;

public class MeetingGroupProposalRejectionMustHaveAReasonRule implements BusinessRule {

    private final String reason;

    public MeetingGroupProposalRejectionMustHaveAReasonRule(String reason) {
        this.reason = reason;
    }

    @Override
    public boolean isBroken() {
        return reason == null || reason.isEmpty();
    }

    @Override
    public String getMessage() {
        return "Meeting group proposal rejection must have a reason";
    }
}
