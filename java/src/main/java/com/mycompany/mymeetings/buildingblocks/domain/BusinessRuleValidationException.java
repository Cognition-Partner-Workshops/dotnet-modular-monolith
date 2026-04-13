package com.mycompany.mymeetings.buildingblocks.domain;

public class BusinessRuleValidationException extends RuntimeException {

    private final BusinessRule brokenRule;
    private final String details;

    public BusinessRuleValidationException(BusinessRule brokenRule) {
        super(brokenRule.getMessage());
        this.brokenRule = brokenRule;
        this.details = brokenRule.getMessage();
    }

    public BusinessRule getBrokenRule() {
        return brokenRule;
    }

    public String getDetails() {
        return details;
    }

    @Override
    public String toString() {
        return brokenRule.getClass().getName() + ": " + brokenRule.getMessage();
    }
}
