package com.mycompany.mymeetings.modules.administration.application.meetinggroupproposals.acceptmeetinggroupproposal;

import com.mycompany.mymeetings.modules.administration.application.contracts.CommandHandler;
import com.mycompany.mymeetings.modules.administration.domain.meetinggroupproposals.MeetingGroupProposal;
import com.mycompany.mymeetings.modules.administration.domain.meetinggroupproposals.MeetingGroupProposalRepository;
import com.mycompany.mymeetings.modules.administration.domain.users.UserContext;
import com.mycompany.mymeetings.modules.administration.domain.users.UserId;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.UUID;

@Service
public class AcceptMeetingGroupProposalCommandHandler implements CommandHandler<AcceptMeetingGroupProposalCommand> {

    private final MeetingGroupProposalRepository meetingGroupProposalRepository;
    private final UserContext userContext;

    public AcceptMeetingGroupProposalCommandHandler(
            MeetingGroupProposalRepository meetingGroupProposalRepository,
            UserContext userContext) {
        this.meetingGroupProposalRepository = meetingGroupProposalRepository;
        this.userContext = userContext;
    }

    @Override
    @Transactional
    public void handle(AcceptMeetingGroupProposalCommand command) {
        UUID proposalId = command.getMeetingGroupProposalId();
        MeetingGroupProposal meetingGroupProposal = meetingGroupProposalRepository.findById(proposalId)
                .orElseThrow(() -> new IllegalArgumentException(
                        "Meeting group proposal not found: " + proposalId));

        meetingGroupProposal.accept(userContext.getUserId());

        meetingGroupProposalRepository.save(meetingGroupProposal);
    }
}
