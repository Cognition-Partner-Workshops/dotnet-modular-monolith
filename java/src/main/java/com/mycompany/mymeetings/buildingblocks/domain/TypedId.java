package com.mycompany.mymeetings.buildingblocks.domain;

import java.io.Serializable;
import java.util.Objects;
import java.util.UUID;

public abstract class TypedId implements Serializable {

    private final UUID value;

    protected TypedId(UUID value) {
        if (value == null || value.equals(new UUID(0, 0))) {
            throw new IllegalArgumentException("Id value cannot be empty!");
        }
        this.value = value;
    }

    public UUID getValue() {
        return value;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        TypedId typedId = (TypedId) o;
        return Objects.equals(value, typedId.value);
    }

    @Override
    public int hashCode() {
        return Objects.hash(value);
    }

    @Override
    public String toString() {
        return value.toString();
    }
}
