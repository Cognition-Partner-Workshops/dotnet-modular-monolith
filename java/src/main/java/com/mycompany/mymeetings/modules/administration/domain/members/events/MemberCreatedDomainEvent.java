package com.mycompany.mymeetings.modules.administration.domain.members.events;

import com.mycompany.mymeetings.buildingblocks.domain.AbstractDomainEvent;
import com.mycompany.mymeetings.modules.administration.domain.members.MemberId;

public class MemberCreatedDomainEvent extends AbstractDomainEvent {

    private final MemberId memberId;

    public MemberCreatedDomainEvent(MemberId memberId) {
        this.memberId = memberId;
    }

    public MemberId getMemberId() {
        return memberId;
    }
}
