package com.mycompany.mymeetings.modules.administration.domain.meetinggroupproposals.events;

import com.mycompany.mymeetings.buildingblocks.domain.AbstractDomainEvent;
import com.mycompany.mymeetings.modules.administration.domain.meetinggroupproposals.MeetingGroupProposalId;

public class MeetingGroupProposalRejectedDomainEvent extends AbstractDomainEvent {

    private final MeetingGroupProposalId meetingGroupProposalId;

    public MeetingGroupProposalRejectedDomainEvent(MeetingGroupProposalId meetingGroupProposalId) {
        this.meetingGroupProposalId = meetingGroupProposalId;
    }

    public MeetingGroupProposalId getMeetingGroupProposalId() {
        return meetingGroupProposalId;
    }
}
