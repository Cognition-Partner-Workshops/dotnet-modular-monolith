package com.mycompany.mymeetings.modules.administration.domain.meetinggroupproposals.rules;

import com.mycompany.mymeetings.buildingblocks.domain.BusinessRule;
import com.mycompany.mymeetings.modules.administration.domain.meetinggroupproposals.MeetingGroupProposalDecision;

public class MeetingGroupProposalCanBeVerifiedOnceRule implements BusinessRule {

    private final MeetingGroupProposalDecision actualDecision;

    public MeetingGroupProposalCanBeVerifiedOnceRule(MeetingGroupProposalDecision actualDecision) {
        this.actualDecision = actualDecision;
    }

    @Override
    public boolean isBroken() {
        return !actualDecision.equals(MeetingGroupProposalDecision.noDecision());
    }

    @Override
    public String getMessage() {
        return "Meeting group proposal can be verified only once";
    }
}
