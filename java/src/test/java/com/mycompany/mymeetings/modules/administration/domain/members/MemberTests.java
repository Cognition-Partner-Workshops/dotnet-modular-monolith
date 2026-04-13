package com.mycompany.mymeetings.modules.administration.domain.members;

import com.mycompany.mymeetings.buildingblocks.domain.BaseEntity;
import com.mycompany.mymeetings.buildingblocks.domain.DomainEvent;
import com.mycompany.mymeetings.modules.administration.domain.members.events.MemberCreatedDomainEvent;
import org.junit.jupiter.api.Test;

import java.util.List;
import java.util.UUID;

import static org.assertj.core.api.Assertions.assertThat;

class MemberTests {

    @Test
    void createMember_isSuccessful() {
        MemberId memberId = new MemberId(UUID.randomUUID());
        Member member = Member.create(
                memberId.getValue(),
                "memberLogin",
                "memberEmail@mail.com",
                "John",
                "Doe",
                "John Doe");

        MemberCreatedDomainEvent event = assertPublishedDomainEvent(member, MemberCreatedDomainEvent.class);
        assertThat(event.getMemberId()).isEqualTo(memberId);
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
}
