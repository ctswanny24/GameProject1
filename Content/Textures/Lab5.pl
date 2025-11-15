% Lab5: Prolog Lab
% Caden Swanson

repetition([X, X | _]). % Checks to see if a repetition exists in the list. 
repetition([_ | Rest]) :-  % If no repetition exists, it recurses to check the tail of that list for the repetition.
    repetition(Rest).      % Repetition is called again for the rest of the list.

subsequence([], []).          
subsequence([Head|Tail1], [Head|Tail2]) :- % If the heads of both of the lists match, it goes into the tail of both lists.
    subsequence(Tail1, Tail2).       % That recursion happens here.
subsequence(List1, [_|Tail2]) :-        %If the heads do not match, you recurse into the tail of the second list to continue to find a match.
    subsequence(List1, Tail2).    

leftSpine(leaf(Val), [Val]).     % If the call has a leaf, it is added to the list.
leftSpine(node(Left, Val, Right), [Val | Rest]) :- % IF the tree ends up being a node, the value is added to the list, then recursively goes down the left side.
    leftSpine(Left, Rest).     % This recursively goes down the left section of the tree. 