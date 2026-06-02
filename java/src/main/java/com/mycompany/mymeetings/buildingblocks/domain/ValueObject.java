package com.mycompany.mymeetings.buildingblocks.domain;

import java.lang.reflect.Field;
import java.util.ArrayList;
import java.util.List;
import java.util.Objects;

public abstract class ValueObject {

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;

        List<Field> fields = getAllFields();
        for (Field field : fields) {
            field.setAccessible(true);
            try {
                Object thisValue = field.get(this);
                Object otherValue = field.get(o);
                if (!Objects.equals(thisValue, otherValue)) {
                    return false;
                }
            } catch (IllegalAccessException e) {
                throw new RuntimeException("Failed to compare value object fields", e);
            }
        }
        return true;
    }

    @Override
    public int hashCode() {
        List<Field> fields = getAllFields();
        int hash = 17;
        for (Field field : fields) {
            field.setAccessible(true);
            try {
                Object value = field.get(this);
                hash = hash * 23 + (value != null ? value.hashCode() : 0);
            } catch (IllegalAccessException e) {
                throw new RuntimeException("Failed to compute value object hash code", e);
            }
        }
        return hash;
    }

    protected static void checkRule(BusinessRule rule) {
        if (rule.isBroken()) {
            throw new BusinessRuleValidationException(rule);
        }
    }

    private List<Field> getAllFields() {
        List<Field> fields = new ArrayList<>();
        Class<?> clazz = getClass();
        while (clazz != null && clazz != ValueObject.class) {
            for (Field field : clazz.getDeclaredFields()) {
                fields.add(field);
            }
            clazz = clazz.getSuperclass();
        }
        return fields;
    }
}
