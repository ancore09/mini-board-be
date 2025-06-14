-- Add user_id column to board_tasks table
ALTER TABLE board_tasks ADD COLUMN user_id BIGINT;

-- Create index for better performance
CREATE INDEX idx_board_tasks_user_id ON board_tasks(user_id);