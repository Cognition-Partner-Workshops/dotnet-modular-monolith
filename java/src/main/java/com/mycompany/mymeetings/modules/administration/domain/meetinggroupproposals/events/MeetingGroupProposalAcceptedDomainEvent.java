package com.mycompany.mymeetings.modules.administration.domain.meetinggroupproposals.events;

import com.mycompany.mymeetings.buildingblocks.domain.AbstractDomainEvent;
import com.mycompany.mymeetings.modules.administration.domain.meetinggroupproposals.MeetingGroupProposalId;

public class MeetingGroupProposalAcceptedDomainEvent extends AbstractDomainEvent {

    private final MeetingGroupProposalId meetingGroupProposalId;

    public MeetingGroupProposalAcceptedDomainEvent(MeetingGroupProposalId meetingGroupProposalId) {
        this.meetingGroupProposalId = meetingGroupProposalId;
    }

    public MeetingGroupProposalId getMeetingGroupProposalId() {
        return meetingGroupProposalId;
    }
}
