package com.mycompany.mymeetings.buildingblocks.domain;

import jakarta.persistence.MappedSuperclass;
import jakarta.persistence.Transient;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

@MappedSuperclass
public abstract class BaseEntity {

    @Transient
    private List<DomainEvent> domainEvents;

    public List<DomainEvent> getDomainEvents() {
        if (domainEvents == null) {
            return Collections.emptyList();
        }
        return Collections.unmodifiableList(domainEvents);
    }

    public void clearDomainEvents() {
        if (domainEvents != null) {
            domainEvents.clear();
        }
    }

    protected void addDomainEvent(DomainEvent domainEvent) {
        if (domainEvents == null) {
            domainEvents = new ArrayList<>();
        }
        domainEvents.add(domainEvent);
    }

    protected void checkRule(BusinessRule rule) {
        if (rule.isBroken()) {
            throw new BusinessRuleValidationException(rule);
        }
    }
}
