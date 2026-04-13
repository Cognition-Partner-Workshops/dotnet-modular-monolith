package com.mycompany.mymeetings.modules.administration.domain.meetinggroupproposals;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.UUID;

@Repository
public interface MeetingGroupProposalRepository extends JpaRepository<MeetingGroupProposal, UUID> {
}
