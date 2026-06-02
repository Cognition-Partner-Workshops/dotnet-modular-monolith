package com.mycompany.mymeetings.modules.administration.domain.members;

import com.mycompany.mymeetings.buildingblocks.domain.AggregateRoot;
import com.mycompany.mymeetings.buildingblocks.domain.BaseEntity;
import com.mycompany.mymeetings.modules.administration.domain.members.events.MemberCreatedDomainEvent;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.Id;
import jakarta.persistence.Table;

import java.time.LocalDateTime;
import java.util.UUID;

@Entity
@Table(name = "Members", schema = "administration")
public class Member extends BaseEntity implements AggregateRoot {

    @Id
    @Column(name = "Id")
    private UUID id;

    @Column(name = "Login")
    private String login;

    @Column(name = "Email")
    private String email;

    @Column(name = "FirstName")
    private String firstName;

    @Column(name = "LastName")
    private String lastName;

    @Column(name = "Name")
    private String name;

    @Column(name = "CreateDate")
    private LocalDateTime createDate;

    protected Member() {
    }

    private Member(UUID id, String login, String email, String firstName, String lastName, String name) {
        this.id = id;
        this.login = login;
        this.email = email;
        this.firstName = firstName;
        this.lastName = lastName;
        this.name = name;
        this.createDate = LocalDateTime.now();

        this.addDomainEvent(new MemberCreatedDomainEvent(getId()));
    }

    public MemberId getId() {
        return new MemberId(id);
    }

    public static Member create(UUID id, String login, String email, String firstName, String lastName, String name) {
        return new Member(id, login, email, firstName, lastName, name);
    }
}
